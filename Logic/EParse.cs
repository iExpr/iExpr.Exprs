using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Exceptions;
using iExpr.Parser;
using iExpr.Values;

namespace iExpr.Exprs.Logic
{
    internal class BasicTokenChecker : TokenChecker
    {
        public override bool? Test(char c)
        {
            if (Flag != null) return false;
            return c=='0' || c=='1';
        }
    }

    public class EParse : Parser.ParseEnvironment
    {
        public EParse()
        {
            base.Operations = new OperationList();
            base.Operations.Add(Operators.And);
            base.Operations.Add(Operators.Or);
            base.Operations.Add(Operators.Xor);
            base.Operations.Add(Operators.Not);
            base.Operations.Add(Operators.Imply);
            base.Operations.Add(Operators.Same);
            base.Operations.Add(iExpr.Exprs.Core.CoreOperations.Lambda);
            base.VariableChecker = new VariableTokenChecker();
            base.BasicTokenChecker = new BasicTokenChecker();
            base.Constants = new ConstantList();
            Constants.Add(new ConstantToken("true", new ReadOnlyConcreteValue(true)));
            Constants.Add(new ConstantToken("false", new ReadOnlyConcreteValue(false)));
            Constants.Add(new ConstantToken("True", new ReadOnlyConcreteValue(true)));
            Constants.Add(new ConstantToken("False", new ReadOnlyConcreteValue(false)));
            base.BuildOpt();
        }

        public override IValue GetBasicValue(Symbol symbol)
        {
            switch (symbol.Value)
            {
                case "1":
                    return new ConcreteValue(true);
                case "0":
                    return new ConcreteValue(false);
                default:
                    throw new UndefinedExecuteException();
            }
        }
    }
}
