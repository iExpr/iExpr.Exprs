using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Math
{
    public class NumberTheoryOperations
    {
        static long gcd(long a, long b)
        {
            return b == 0 ? a : gcd(b, a % b);
        }

        static long quick_pow(long a, long n, long mod)
        {
            long ans = 1;
            a %= mod;
            while (n > 0)
            {
                if ((n & 1) == 1) ans = (ans * a) % mod;
                a = (a * a) % mod;
                n >>= 1;
            }
            return ans;
        }
        /*

        /// <summary>
        /// 求模运算下的乘法逆元
        /// </summary>
        public static PreFunctionValue Inv { get; } = new PreFunctionValue(
            "invn",
            (IExpr[] args, EvalContext cal) =>
            {
                if (!OperationHelper.AssertConstantValue(args))
                    return new ExprNode(Gcd, args);
                var ov = OperationHelper.GetConcreteValue<long>(args);
                return ConcreteValueHelper.BuildValue(gcd(ov[0], ov[1]));
            },
            2);
            */

        /// <summary>
        /// 计算最大公因数
        /// </summary>
        public static PreFunctionValue Gcd { get; } = new PreFunctionValue(
            "gcd",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = OperationHelper.GetValue<long>(args);
                return new ConcreteValue(gcd(ov[0], ov[1]));
            },
            2);

        /// <summary>
        /// 计算最小公倍数
        /// </summary>
        public static PreFunctionValue Lcm { get; } = new PreFunctionValue(
            "lcm",
            (FunctionArgument _args, EvalContext cal) =>
            {
                var args = _args.Arguments;
                OperationHelper.AssertArgsNumberThrowIf(2, args);
                OperationHelper.AssertCertainValueThrowIf(args);
                var ov = OperationHelper.GetValue<long>(args);
                long g = gcd(ov[0], ov[1]);
                return new ConcreteValue(ov[0]/g*ov[1]);
            },
            2);
    }
}
