using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using iExpr.Evaluators;
using iExpr.Values;

namespace iExpr.Exprs.Math
{
    public class EEContext : EvalContext
    {
        protected EEContext() { }

        public override EvalContext GetChild(VariableFindMode mode = VariableFindMode.UpAll)
        {
            return new EEContext() { Evaluator = Evaluator, CancelToken = CancelToken, Parent = this };
        }

        protected override T ConvertValue<T>(object val)
        {
            if (typeof(T).IsAssignableFrom(typeof(RealNumber)))
            {
                try
                {
                    double d = Convert.ToDouble(val);
                    return (T)(object)(new RealNumber(d));
                }
                catch { }
            }
            if (val is RealNumber)
            {
                try
                {
                    return (T)Convert.ChangeType((double)((RealNumber)val), typeof(T));
                }
                catch { }
            }
            return base.ConvertValue<T>(val);
        }

        public new static EvalContext Create(CancellationTokenSource cancel)
        {
            var res = new EEContext
            {
                CancelToken = cancel
            };
            return res;
        }
    }

    public class EEval : Evaluators.EvalEnvironment
    {
        public EEval()
        {
            base.Evaluator = new Evaluators.ExprEvaluator();
        }

        public override EvalContext CreateContext(CancellationTokenSource cancel = null)
        {
            var res = EEContext.Create(cancel ?? new System.Threading.CancellationTokenSource());
            res.Evaluator = Evaluator;
            res.Variables = Variables;
            return res;
        }
    }
}
