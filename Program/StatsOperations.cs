using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Program
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
