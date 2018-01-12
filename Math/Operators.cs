using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Math
{
    public static class Operators
    {
        /// <summary>
        /// 加法
        /// </summary>
        public static Operator Plus { get; } = new Operator(
            "+",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Plus, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] + ov[1]);
            },
            null,
            (double)Priority.Midium,
            Association.Left,
            2);

        /// <summary>
        /// 减法
        /// </summary>
        public static Operator Minus { get; } = new Operator(
            "-",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Minus, args);
                var ov = OperationHelper.GetValue<double>(args);
                    return new ConcreteValue(ov[0] - ov[1]);
            },
            (IExpr[] args) =>
            {
                    return string.Join("-", args.Select((IExpr exp) => Operator.BlockToString(exp)));
            },
        (double)Priority.Midium,
            Association.Left,
            2);

        /// <summary>
        /// 乘法
        /// </summary>
        public static Operator Multiply { get; } = new Operator(
            "*",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Multiply, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] * ov[1]);
            },
            null,
            (double)Priority.MIDIUM,
            Association.Left,
            2);

        /// <summary>
        /// 除法
        /// </summary>
        public static Operator Divide { get; } = new Operator(
            "/",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Divide, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] / ov[1]);
            },
            null,
            (double)Priority.MIDIUM,
            Association.Left,
            2);

        /// <summary>
        /// 模运算
        /// </summary>
        public static Operator Mod { get; } = new Operator(
           "%",
           (IExpr[] args, EvalContext cal) =>
           {
               OperationHelper.AssertArgsNumberThrowIf(2, args);
               if (!OperationHelper.AssertConstantValue(args))
                   return new ExprNodeBinaryOperation(Mod, args);
               var ov = OperationHelper.GetValue<double>(args);
               return new ConcreteValue(ov[0] % ov[1]);
           },
           null,
           (double)Priority.MIDIUM,
           Association.Left,
           2);

        /// <summary>
        /// 乘方运算
        /// </summary>
        public static Operator Pow { get; } = new Operator(
            "^",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Pow, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(System.Math.Pow(ov[0], ov[1]));
            },
            null,
            (double)Priority.high,
            Association.Right,
            2);

        /// <summary>
        /// 上取整
        /// </summary>
        public static PreFunctionValue Ceil { get; } = new PreFunctionValue(
            "ceil",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Ceil, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Ceiling(ov));
            },
            1);

        /// <summary>
        /// 下取整
        /// </summary>
        public static PreFunctionValue Floor { get; } = new PreFunctionValue(
            "floor",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Floor, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Floor(ov));
            },
            1);

        /// <summary>
        /// 舍入取整
        /// </summary>
        public static PreFunctionValue Round { get; } = new PreFunctionValue(
            "round",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Round, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Round(ov));
            },
            1);

        /// <summary>
        /// 符号函数
        /// </summary>
        public static PreFunctionValue Sign { get; } = new PreFunctionValue(
            "sign",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Sign, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Sign(ov));
            },
            1);

        /// <summary>
        /// e的幂次
        /// </summary>
        public static PreFunctionValue Exp { get; } = new PreFunctionValue(
            "exp",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Exp, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Exp(ov));
            },
            1);

        /// <summary>
        /// 相等
        /// </summary>
        public static Operator Equal { get; } = new Operator(
            "==",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Equal, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0]==ov[1]);
            },
            (IExpr[] args) => string.Join("==", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 不等
        /// </summary>
        public static Operator Unequal { get; } = new Operator(
            "!=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Unequal, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] != ov[1]);
            },
            (IExpr[] args) => string.Join("!=", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 大于
        /// </summary>
        public static Operator Bigger { get; } = new Operator(
            ">",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Bigger, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] > ov[1]);
            },
            (IExpr[] args) => string.Join(">", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 小于
        /// </summary>
        public static Operator Smaller { get; } = new Operator(
            "<",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(Smaller, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] < ov[1]);
            },
            (IExpr[] args) => string.Join("<", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 大于等于
        /// </summary>
        public static Operator NotSmaller { get; } = new Operator(
            ">=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(NotSmaller, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] >= ov[1]);
            },
            (IExpr[] args) => string.Join(">=", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 小于等于
        /// </summary>
        public static Operator NotBigger { get; } = new Operator(
            "<=",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeBinaryOperation(NotBigger, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(ov[0] <= ov[1]);
            },
            (IExpr[] args) => string.Join("<=", args.Select((IExpr exp) => Operator.BlockToString(exp))),
            (double)Priority.LOW,
            Association.Left,
            2);

        /// <summary>
        /// 非运算
        /// </summary>
        public static Operator Not { get; } = new Operator(
            "~",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args)) return new ExprNodeSingleOperation(Not, args[0]);
                var p = OperationHelper.GetValue<bool>(args[0]);
                return new ConcreteValue(!p);
            },
            (IExpr[] args) => $"~{Operator.BlockToString(args[0])}",
            (double)Priority.High,
            Association.Right,
            1);

        /// <summary>
        /// 阶乘运算
        /// </summary>
        public static Operator Factorial { get; } = new Operator(
            "!",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args)) return new ExprNodeSingleOperation(Factorial, args[0]);
                var p = OperationHelper.GetValue<int>(args[0]);
                double s = 1;
                for (int i = p; i >= 2; i--) s *= i;
                return new ConcreteValue(s);
            },
            (IExpr[] args) => $"{Operator.BlockToString(args[0])}!",
            (double)Priority.High,
            Association.Left,
            1);

        /// <summary>
        /// 绝对值
        /// </summary>
        public static PreFunctionValue Abs { get; } = new PreFunctionValue(
            "abs",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Abs, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Abs(ov));
            },
            1);

        /// <summary>
        /// 正弦函数
        /// </summary>
        public static PreFunctionValue Sin { get; } = new PreFunctionValue(
            "sin",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Sin, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Sin(ov));
            },
            1);

        /// <summary>
        /// 余弦函数
        /// </summary>
        public static PreFunctionValue Cos { get; } = new PreFunctionValue(
            "cos",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Cos, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Cos(ov));
            }, 1);

        /// <summary>
        /// 正切函数
        /// </summary>
        public static PreFunctionValue Tan { get; } = new PreFunctionValue(
            "tan",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Tan, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Tan(ov));
            }, 1);

        /// <summary>
        /// 反正弦函数
        /// </summary>
        public static PreFunctionValue ArcSin { get; } = new PreFunctionValue(
            "arcsin",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(ArcSin, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Asin(ov));
            },
            1);

        /// <summary>
        /// 反余弦函数
        /// </summary>
        public static PreFunctionValue ArcCos { get; } = new PreFunctionValue(
            "arccos",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(ArcCos, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Acos(ov));
            },
            1);

        /// <summary>
        /// 反正切函数
        /// </summary>
        public static PreFunctionValue ArcTan { get; } = new PreFunctionValue(
            "arctan",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(ArcTan, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Atan(ov));
            },
            1);

        /// <summary>
        /// 自然对数
        /// </summary>
        public static PreFunctionValue Ln { get; } = new PreFunctionValue(
            "ln",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Ln, args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Log(ov));
            }, 1);

        /// <summary>
        /// 对数
        /// </summary>
        public static PreFunctionValue Log { get; } = new PreFunctionValue(
            "log",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNodeCall(Log, args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(System.Math.Log(ov[1]) / System.Math.Log(ov[0]));
            },
            2);
    }
}
