using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Program.Helpers
{
    public class RuntimeBreak : Exception
    {

    }

    public class BReturn : RuntimeBreak
    {
        public IExpr Value { get; set; }

        public BReturn(IExpr val)
        {
            Value = val;
        }
    }

    public class BBreak : RuntimeBreak
    {

    }

    public class BContinue : RuntimeBreak
    {

    }
}
