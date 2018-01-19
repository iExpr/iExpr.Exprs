using iExpr;
using iExpr.Evaluators;
using iExpr.Exceptions;
using iExpr.Exprs.Core;
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

        /// <summary>
        /// Build a list
        /// </summary>
        public static PreFunctionValue List { get; } = new PreFunctionValue(
            "list",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;

                return new ListValue(iExpr.Exprs.Core.CoreOperations.BuildValueList(args));
            });

        /// <summary>
        /// Build a set
        /// </summary>
        public static PreFunctionValue Set { get; } = new PreFunctionValue(
            "set",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                return new SetValue(iExpr.Exprs.Core.CoreOperations.BuildValueList(args));
            });

        /// <summary>
        /// Build a tuple
        /// </summary>
        public static PreFunctionValue Tuple { get; } = new PreFunctionValue(
            "tuple",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;

                return new TupleValue(iExpr.Exprs.Core.CoreOperations.BuildValueList(args));
            });

        public static PreFunctionValue Array { get; } = new PreFunctionValue(
                "array",
                (FunctionArgument _args, EvalContext cal) =>
                {
                    var args = _args.Arguments;
                    if (OperationHelper.AssertArgsNumber(1, args))
                    {
                        OperationHelper.AssertCertainValueThrowIf(Array,args[0]);
                        int n = cal.GetValue<int>(args[0]);
                        return new TupleValue(new IValue[n]);
                    }
                    else if (OperationHelper.AssertArgsNumber(2, args))
                    {
                        OperationHelper.AssertCertainValueThrowIf(Array,args[0]);
                        int n = cal.GetValue<int>(args[0]);
                        List<IValue> exps = new List<IValue>();
                        if (args[1] is IValue)
                        {
                            for (int i = 0; i < n; i++) exps.Add((IValue)args[1]);//TODO: this is ref not clone!
                            return new TupleValue(exps);
                        }
                        else ExceptionHelper.RaiseNotValue(Array, args[1]);
                            
                    }
                    else
                    {
                        ExceptionHelper.RaiseWrongArgsNumber(Array, 2, args?.Length ?? 0);
                    }
                    return default;
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
                -1,false, new EvalContextStartupInfo(true, false)
                );

        public static IExpr AssignFunc(object sender,object v,object val,EvalContext cal)
        {
            switch (v)
            {
                case ConcreteValue x://可能是列表中的元素，也可能得到常量列表中的ReadOnlyValue，但会赋值错误
                    x.Value = val;
                    return x;
                case VariableToken x:
                    {
                        var t = val is ConcreteValue ? (ConcreteValue)val : new ConcreteValue(val);
                        cal.SetVariableValue(x.ID, t);
                        return t;
                    }
                default:
                    ExceptionHelper.RaiseInvalidExpressionFailed(sender, v as IExpr, "The left expr of assign is not supported.");
                    return default;
            }
        }

        /// <summary>
        /// 赋值
        /// </summary>
        public static Operator Assign { get; } = new Operator(
            "=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Assign,2, args);
                OperationHelper.AssertCertainValueThrowIf(Assign, args[1]);
                switch (args[0])
                {
                    case CollectionValue c:
                        var cv = cal.GetValue<IEnumerableValue>(args[1]);
                        if (cv == null) ExceptionHelper.RaiseInvalidExpressionFailed(Assign, args[1], "Expect a enumerable to assign.");
                        var it = cv.GetEnumerator();
                        List<IValue> res = new List<IValue>();
                        foreach (var k in c)
                        {
                            if (it.MoveNext() == false) ExceptionHelper.RaiseInvalidExpressionFailed(Assign, args[1], "Don't have enough value to assign.");
                            var vr = cal.Evaluate((IExpr)k.Value);//NativeExprValue
                            res.Add((IValue)AssignFunc(Assign, vr, OperationHelper.GetValue(it.Current), cal));
                        }
                        var r = c.CreateNew();r.Reset(res);
                        return r;
                    default:
                        {
                            var vr = cal.Evaluate(args[0]);
                            var av = OperationHelper.GetValue(args[1]);
                            //TODO: 这里没有检查IValue要求
                            return AssignFunc(Assign, vr, av, cal);
                        }
                }
                
            }, null, (double)Priority.lowest, Association.Right, 2, OperationHelper.GetSelfCalculate(0)
            );


        public static Operator AssignPlus { get; } = new Operator(
            "+=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(AssignPlus, 2, args);
                OperationHelper.AssertCertainValueThrowIf(AssignPlus, args);
                var ov = cal.GetValue<IAdditive>(args);
                var res = ov[0].Add(ov[1]);
                return AssignFunc(AssignPlus, args[0], res, cal);
            }, null, (double)Priority.lowest, Association.Right, 2
            );

        public static Operator AssignMinus { get; } = new Operator(
            "-=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(AssignMinus, 2, args);
                OperationHelper.AssertCertainValueThrowIf(AssignMinus, args);
                var ov = cal.GetValue<ISubtractive>(args);
                var res = ov[0].Subtract(ov[1]);
                return AssignFunc(AssignMinus, args[0], res, cal);
            }, null, (double)Priority.lowest, Association.Right, 2
            );

        public static Operator AssignMultiply { get; } = new Operator(
            "*=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(AssignMultiply, 2, args);
                OperationHelper.AssertCertainValueThrowIf(AssignMultiply, args);
                var ov = cal.GetValue<IMultiplicable>(args);
                var res = ov[0].Multiply(ov[1]);
                return AssignFunc(AssignMultiply, args[0], res, cal);
            }, null, (double)Priority.lowest, Association.Right, 2
            );

        public static Operator AssignDivide { get; } = new Operator(
            "/=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(AssignDivide, 2, args);
                OperationHelper.AssertCertainValueThrowIf(AssignDivide, args);
                var ov = cal.GetValue<IDivisible>(args);
                var res = ov[0].Divide(ov[1]);
                return AssignFunc(AssignDivide, args[0], res, cal);
            }, null, (double)Priority.lowest, Association.Right, 2
            );

        public static Operator AssignMod { get; } = new Operator(
            "%=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(AssignMod, 2, args);
                OperationHelper.AssertCertainValueThrowIf(AssignMod, args);
                var ov = cal.GetValue<IMouldable>(args);
                var res = ov[0].Mod(ov[1]);
                return AssignFunc(AssignMod, args[0], res, cal);
            }, null, (double)Priority.lowest, Association.Right, 2
            );

        public static Operator AssignPow { get; } = new Operator(
            "**=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(AssignPow, 2, args);
                OperationHelper.AssertCertainValueThrowIf(AssignPow, args);
                var ov = cal.GetValue<IPowerable>(args);
                var res = ov[0].Pow(ov[1]);
                return AssignFunc(AssignPow, args[0], res, cal);
            }, null, (double)Priority.lowest, Association.Right, 2
            );

        static IExpr ReAssignFunc(object sender,object v,object val,EvalContext cal)
        {
            if (!(v is VariableToken)) ExceptionHelper.RaiseInvalidExpressionFailed(sender, v as IExpr, "The left must be a variable.");
            string id = (v as VariableToken).ID;
            var t = val is ConcreteValue ? (ConcreteValue)val : new ConcreteValue(val);
            cal.Variables.Set(id, t);
            return t;
        }

        /// <summary>
        /// 创建式赋值（将隐藏掉上层同名变量）
        /// </summary>
        public static Operator ReAssign { get; } = new Operator(
            ":=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Assign, 2, args);
                OperationHelper.AssertCertainValueThrowIf(Assign, args[1]);
                switch (args[0])
                {
                    case CollectionValue c:
                        var cv = cal.GetValue<IEnumerableValue>(args[1]);
                        if (cv == null) ExceptionHelper.RaiseInvalidExpressionFailed(Assign, args[1], "Expect a enumerable to assign.");
                        var it = cv.GetEnumerator();
                        List<IValue> res = new List<IValue>();
                        foreach (var k in c)
                        {
                            if (it.MoveNext() == false) ExceptionHelper.RaiseInvalidExpressionFailed(ReAssign, args[1], "Don't have enough value to assign.");
                            var vr = cal.Evaluate((IExpr)k.Value);//NativeExprValue
                            res.Add((IValue)ReAssignFunc(ReAssign, vr, OperationHelper.GetValue(it.Current), cal));
                        }
                        var r = c.CreateNew(); r.Reset(res);
                        return r;
                    default://NativeExprValue
                        {
                            var vr = args[0];//variableToken won't be a nativeExprValue
                            var av = OperationHelper.GetValue(args[1]);
                            //TODO: 这里没有检查IValue要求
                            return ReAssignFunc(ReAssign, vr, av, cal);
                        }
                }
            }, null, (double)Priority.lowest, Association.Right, 2, OperationHelper.GetSelfCalculate(0)
            );


    }
}
