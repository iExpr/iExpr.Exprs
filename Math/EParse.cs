using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Exceptions;
using iExpr.Parser;
using iExpr.Values;

namespace iExpr.Exprs.Math
{
    internal class BasicTokenChecker : TokenChecker
    {
        int pointcnt = 0;

        public override void Clear()
        {
            base.Clear();pointcnt = 0;
        }

        public override bool Append(char c)
        {
            var res=base.Append(c);
            if (c == '.') pointcnt++;
            return res;
        }

        public override bool Test(char c)
        {
            if (Flag == null)
                return char.IsDigit(c);
            if (c == '.') return pointcnt == 0;
            return char.IsDigit(c);
        }
    }

    public class EParse : Parser.ParseEnvironment
    {
        public EParse()
        {
            base.Operations = new OperationList();
            base.Operations.Add(Operators.Plus);
            base.Operations.Add(Operators.Minus);
            base.Operations.Add(Operators.Multiply);
            base.Operations.Add(Operators.Divide);
            base.Operations.Add(Operators.Mod);
            base.Operations.Add(Operators.Pow);
            base.Operations.Add(iExpr.Exprs.Core.CoreOperations.Lambda);
            base.VariableChecker = new VariableTokenChecker();
            base.BasicTokenChecker = new BasicTokenChecker();
            base.Constants = new ConstantList();
            Constants.Add(new ConstantToken("e", new ConcreteValue(System.Math.E)));
            Constants.Add(new ConstantToken("pi", new ConcreteValue(System.Math.PI)));
            base.BuildOpt();
        }

        public override IValue GetBasicValue(Symbol symbol)
        {
            return new ConcreteValue(double.Parse(symbol.Value));
        }
    }
}
