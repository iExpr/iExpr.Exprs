using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Program
{
    public static class BasicOperations
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
            "**",
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
    }
}
