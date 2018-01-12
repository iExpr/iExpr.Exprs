using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Math
{
    public static class StatsOperations
    {
        internal static List<double> GetAll(IExpr[] args, EvalContext context)
        {
            List<IExpr> ls = new List<IExpr>();

            foreach (var v in args)
            {
                switch (v)
                {
                    case CollectionValue c:
                        ls.AddRange(c);
                        break;
                    default:
                        ls.Add(v);
                        break;
                }
            }
            return new List<double>(ls.Select(x=>OperationHelper.GetValue<double>(x)));
        }

        /// <summary>
        /// 单变量表达式枚举求和
        /// </summary>
        public static PreFunctionValue Sum { get; } = new PreFunctionValue(
            "sum",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(3, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Sum, args);
                var ov = OperationHelper.GetValue<int>(args[1], args[2]);
                var func = OperationHelper.GetValue<FunctionValue>(args[0]);
                int a = ov[0], b = ov[1];
                double res = 0;
                if (func.ArgumentCount == 1)
                {
                    //Parallel
                    res = Enumerable.Range(a, b - a + 1).AsParallel().WithCancellation(cal.CancelToken.Token)
                .Select(i =>
                {
                    try
                    {
                        double r = OperationHelper.GetValue<double>(func.EvaluateFunc(new FunctionArgument(new ConcreteValue(i)),cal));
                        return r;
                    }
                    catch
                    {
                        return double.NaN;
                    }
                }).Sum();
                    /*wk.Variables.Add(id, 0);
                    for (int i = a; i <= b; i++)
                    {
                        wk.Variables[id] = i;
                        double r = ConstantValueHelper.GetValue<double>(wk.Calculate(exp));
                        res += r;
                    }*/
                }
                else if (func.ArgumentCount == 0)
                {
                    double r = OperationHelper.GetValue<double>(func.EvaluateFunc(new FunctionArgument(), cal));
                    res += r * (b - a + 1);
                }
                else return new ExprNodeCall(Sum, args);//包含多个变量
                return new ConcreteValue(res);
            },
            3
            );

        /// <summary>
        /// 单变量表达式枚举求积
        /// </summary>
        public static PreFunctionValue Product { get; } = new PreFunctionValue(
            "prod",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(3, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Sum, args);
                var ov = OperationHelper.GetValue<int>(args[1], args[2]);
                var func = OperationHelper.GetValue<FunctionValue>(args[0]);
                int a = ov[0], b = ov[1];
                double res = 1;
                if (func.ArgumentCount == 1)
                {
                    for (long i = a; i <= b; i++)
                    {
                        cal.AssertNotCancel();
                        double r = OperationHelper.GetValue<double>(func.EvaluateFunc(new FunctionArgument(new ConcreteValue(i)), cal));
                        res *= r;
                    }
                }
                else if (func.ArgumentCount == 0)
                {
                    for (int i = a; i <= b; i++)
                    {
                        double r = OperationHelper.GetValue<double>(func.EvaluateFunc(new FunctionArgument(new ConcreteValue(i)), cal));
                        res *= r;
                    }
                }
                else return new ExprNodeCall(Product, args);//包含多个变量
                return new ConcreteValue(res);
            },
            3
            );

        /// <summary>
        /// 最大值
        /// </summary>
        public static PreFunctionValue Maximum { get; } = new PreFunctionValue(
            "max",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Maximum, args);
                var vs = GetAll(args, cal);
                //var vs = OperationHelper.GetConcreteValue<double>(args);

                return new ConcreteValue(vs.AsParallel().WithCancellation(cal.CancelToken.Token).Max());
            }
            );

        /// <summary>
        /// 最小值
        /// </summary>
        public static PreFunctionValue Minimum { get; } = new PreFunctionValue(
            "min",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Minimum, args);
                var vs = GetAll(args, cal);
                //var vs = OperationHelper.GetConcreteValue<double>(args);

                return new ConcreteValue(vs.AsParallel().WithCancellation(cal.CancelToken.Token).Min());
            }
            );

        /// <summary>
        /// 总和
        /// </summary>
        public static PreFunctionValue Total { get; } = new PreFunctionValue(
            "total",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Total, args);
                var vs = GetAll(args, cal);
                //var vs = OperationHelper.GetConcreteValue<double>(args);

                return new ConcreteValue(vs.AsParallel().WithCancellation(cal.CancelToken.Token).Sum());

            }
            );



        /// <summary>
        /// 平均值
        /// </summary>
        public static PreFunctionValue Mean { get; } = new PreFunctionValue(
            "mean",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Mean, args);
                var vs = GetAll(args, cal);
                //var vs = OperationHelper.GetConcreteValue<double>(args);

                return new ConcreteValue(vs.AsParallel().WithCancellation(cal.CancelToken.Token).Average());

            }
            );
    }
}
