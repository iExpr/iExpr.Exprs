using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Math
{
    public class SetOperations
    {
        /// <summary>
        /// 并集
        /// </summary>
        public static PreFunctionValue Cup { get; } = new PreFunctionValue(
            "cup",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (args.Length == 0) return new SetValue();
                OperationHelper.AssertCertainValueThrowIf<CollectionValue>(args);
                var ov = cal.GetValue<CollectionValue>(args);
                HashSet<IValue> set = new HashSet<IValue>(ov[0]);
                for (int i = 1; i < ov.Length; i++)
                {
                    set.UnionWith(ov[i]);
                }
                return new SetValue(set);
            }
            );

        public static PreFunctionValue Cap { get; } = new PreFunctionValue(
            "cap",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (args.Length == 0) return new SetValue();
                OperationHelper.AssertCertainValueThrowIf<CollectionValue>(args);
                var ov = cal.GetValue<CollectionValue>(args);
                HashSet<IValue> set = new HashSet<IValue>(ov[0]);
                for (int i = 1; i < ov.Length; i++)
                {
                    set.IntersectWith(ov[i]);
                }
                return new SetValue(set);
            }
            );

        public static PreFunctionValue Dif { get; } = new PreFunctionValue(
            "dif",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                if (args.Length == 0) return new SetValue();
                OperationHelper.AssertCertainValueThrowIf<CollectionValue>(args);
                var ov = cal.GetValue<CollectionValue>(args);
                HashSet<IValue> set = new HashSet<IValue>(ov[0]);
                for (int i = 1; i < ov.Length; i++)
                {
                    set.ExceptWith(ov[i]);
                }
                return new SetValue(set);
            }
            );

    }
}
