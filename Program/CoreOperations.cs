using iExpr.Evaluators;
using iExpr.Exprs.Program.Helpers;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iExpr.Exprs.Program
{
    public static class CoreOperations
    {
        public static PreFunctionValue Array { get; } = new PreFunctionValue(
                "array",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    var args = _args.Arguments;
                    if (OperationHelper.AssertArgsNumber(1, args))
                    {
                        if (OperationHelper.AssertConstantValue(args[0]))
                        {
                            int n = OperationHelper.GetValue<int>(args[0]);
                            return new TupleValue(new IExpr[n]);
                        }
                        else return new ExprNodeCall(Array, args);
                    }
                    else if (OperationHelper.AssertArgsNumber(2, args))
                    {
                        if (OperationHelper.AssertConstantValue(args[0]))
                        {
                            int n = OperationHelper.GetValue<int>(args[0]);
                            List<IExpr> exps = new List<IExpr>();
                            for (int i = 0; i < n; i++) exps.Add(args[1]);//TODO: this is ref not clone!
                            return new TupleValue(exps);
                        }
                        else return new ExprNodeCall(Array, args);
                    }
                    else
                    {
                        throw new Exception("The number of args is not correct.");
                    }
                }
                );

        public static PreFunctionValue Func { get; } = new PreFunctionValue(
                "func",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    var args = _args.Contents;
                    var c = cal.GetChild();
                    try
                    {
                        foreach (var v in args)
                        {
                            switch (v)
                            {
                                case ExprNode e:
                                    c.Evaluate(e);
                                    break;
                                default:
                                    c.Evaluate(v);
                                    break;
                            }
                        }
                    }
                    catch(BReturn r)
                    {
                        return r.Value;
                    }
                    return BuiltinValues.Null;
                },
                -1
                );

        /// <summary>
        /// 赋值
        /// </summary>
        public static Operator Assign { get; } = new Operator(
            "=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                var left = args[0];
                var a0 = cal.Evaluate(left);
                //TODO: 这里没有检查IValue要求
                switch (a0)
                {
                    case ConcreteValue v://可能是列表中的元素，也可能得到常量列表中的ReadOnlyValue，但会赋值错误
                            v.Value = OperationHelper.GetValue<object>(args[1]);
                            return v;
                    default:
                        if(left is VariableToken)//这是可能是a0可能Function
                        {
                            cal.SetVariableValue((left as VariableToken).ID, args[1]);
                            return args[1];
                        }
                        throw new EvaluateException("The left expr of assign is not supported.");
                }
            }, null, (double)Priority.lowest, Association.Right, 2, OperationHelper.GetSelfCalculate(0)
            );

        /// <summary>
        /// 创建式赋值（将隐藏掉上层同名变量）
        /// </summary>
        public static Operator ReAssign { get; } = new Operator(
            ":=",
            (IExpr[] args, EvalContext cal) =>
            {
                if (!(args[0] is VariableToken)) throw new Exception("The left must be a variable.");
                string id = (args[0] as VariableToken).ID;
                cal.Variables.Set(id, args[1]);//和Assign只有这里不同
                return args[1];
            }, null, (double)Priority.lowest, Association.Right, 2, OperationHelper.GetSelfCalculate(0)
            );
    }
}
