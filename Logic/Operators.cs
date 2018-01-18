using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iExpr.Exprs.Logic
{
    public static class Operators
    {
        /// <summary>
        /// 推出
        /// </summary>
        public static Operator Imply { get; } = new Operator(
            "->",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Imply,2, args);
                OperationHelper.AssertCertainValueThrowIf(Imply,args);
                var bs = cal.GetValue<bool>(args);
                return new ConcreteValue(!bs[0] || bs[1]);
            },
            null,
            (double)Priority.LOWEST,
            Association.Left,
            2);

        /// <summary>
        /// 等价
        /// </summary>
        public static Operator Same { get; } = new Operator(
            "<->",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Same,2, args);
                OperationHelper.AssertCertainValueThrowIf(Same,args);
                var bs = cal.GetValue<bool>(args);
                return new ConcreteValue(bs[0] == bs[1]);
            },
            null,
            (double)Priority.LOWEST,
            Association.Left,
            2);

        /// <summary>
        /// 或运算
        /// </summary>
        public static Operator Or { get; } = new Operator(
            "|",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Or,2, args);
            OperationHelper.AssertCertainValueThrowIf(Or,args);
                var bs = cal.GetValue<bool>(args);
                return new ConcreteValue(bs[0] || bs[1]);
            },
            null,
            (double)Priority.low,
            Association.Left,
            2);

        /// <summary>
        /// 异或运算
        /// </summary>
        public static Operator Xor { get; } = new Operator(
            "^",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Xor,2, args);
            OperationHelper.AssertCertainValueThrowIf(Xor,args);
                var bs = cal.GetValue<bool>(args);
                return new ConcreteValue(bs[0] ^ bs[1]);
            },
            null,
            (double)Priority.low,
            Association.Left,
            2);

        /// <summary>
        /// 且运算
        /// </summary>
        public static Operator And { get; } = new Operator(
            "&",
            (IExpr[] args, EvalContext cal) =>
            {
            OperationHelper.AssertArgsNumberThrowIf(And,2, args);
            OperationHelper.AssertCertainValueThrowIf(And,args);
                var bs = cal.GetValue<bool>(args);
                return new ConcreteValue(bs[0] && bs[1]);
            },
            null,
            (double)Priority.Low,
            Association.Left,
            2);

        /// <summary>
        /// 非运算
        /// </summary>
        public static Operator Not { get; } = new Operator(
            "!",
            (IExpr[] args, EvalContext cal) =>
            {
                OperationHelper.AssertArgsNumberThrowIf(Not,1, args);
                OperationHelper.AssertCertainValueThrowIf(Not,args);
                var p = cal.GetValue<bool>(args[0]);
                return new ConcreteValue(!p);
            },
            (IExpr[] args) => $"!{Operator.BlockToString(args[0])}",
            (double)Priority.High,
            Association.Right,
            1);
    }
}
