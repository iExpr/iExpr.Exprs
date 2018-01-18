using iExpr.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Program.Helpers
{
    public class RuntimeBreak : IgnoredException
    {

    }

    /*public class BYield : RuntimeBreak
    {
        public IExpr Value { get; set; }

        public BYield(IExpr val)
        {
            Value = val;
        }
    }*/

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
