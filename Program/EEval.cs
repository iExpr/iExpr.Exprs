using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Evaluators;

namespace iExpr.Exprs.Program
{
    public class EEval : Evaluators.EvalEnvironment
    {
        public EEval()
        {
            base.Evaluator = new Evaluators.ExprEvaluator();
        }
    }
}
