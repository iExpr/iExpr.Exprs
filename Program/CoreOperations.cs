using iExpr;
using iExpr.Evaluators;
using iExpr.Exceptions;
using iExpr.Exprs.Program.Helpers;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
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
                        OperationHelper.AssertCertainValueThrowIf(args[0]);
                        int n = cal.GetValue<int>(args[0]);
                        return new TupleValue(new IValue[n]);
                    }
                    else if (OperationHelper.AssertArgsNumber(2, args))
                    {
                        OperationHelper.AssertCertainValueThrowIf(args[0]);
                        int n = cal.GetValue<int>(args[0]);
                        List<IValue> exps = new List<IValue>();
                        if (args[1] is IValue)
                        {
                            for (int i = 0; i < n; i++) exps.Add((IValue)args[1]);//TODO: this is ref not clone!
                            return new TupleValue(exps);
                        }
                        else throw new NotValueException();
                            
                    }
                    else
                    {
                        throw new Exception("The number of args is not correct.");
                    }
                }
                );

        public static PreFunctionValue Dict { get; } = new PreFunctionValue(
                "dict",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    return new DictionaryValue();
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
                var av = OperationHelper.GetValue(args[1]);
                switch (a0)
                {
                    case ConcreteValue v://可能是列表中的元素，也可能得到常量列表中的ReadOnlyValue，但会赋值错误
                        v.Value = av;
                        return v;
                    default:
                        if(left is VariableToken)
                        {
                            
                            var t = av is ConcreteValue ? (ConcreteValue)av : new ConcreteValue(av);
                            cal.SetVariableValue((left as VariableToken).ID, t);
                            return t;
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
                var av = OperationHelper.GetValue(args[1]);
                var t= av is ConcreteValue ? (ConcreteValue)av : new ConcreteValue(av);
                cal.Variables.Set(id, t);
                return t;
            }, null, (double)Priority.lowest, Association.Right, 2, OperationHelper.GetSelfCalculate(0)
            );


    }
}
