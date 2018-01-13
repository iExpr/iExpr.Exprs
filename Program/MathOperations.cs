using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Program
{
    public class MathOperations
    {
        /// <summary>
        /// 上取整
        /// </summary>
        public static PreFunctionValue Ceil { get; } = new PreFunctionValue(
            "ceil",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(1, args);
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = OperationHelper.GetValue<double>(args[0]);
                return new ConcreteValue(System.Math.Exp(ov));
            },
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
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
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = OperationHelper.GetValue<double>(args);
                return new ConcreteValue(System.Math.Log(ov[1]) / System.Math.Log(ov[0]));
            },
            2);
    }
}
