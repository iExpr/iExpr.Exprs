using System;
using System.Collections.Generic;
using System.Text;
using iExpr.Exceptions;
using iExpr.Parser;
using iExpr.Values;

namespace iExpr.Exprs.Program
{
    internal class BasicTokenChecker : TokenChecker
    {
        int pointcnt = 0;
        //bool isneg = false;

        public override void Clear()
        {
            base.Clear(); pointcnt = 0;
            //isneg = false;
        }

        public override bool? Append(char c)
        {
            var res = base.Append(c);
            if (c == '.') pointcnt++;
            //if (c == '-') isneg = true;
            return res;
        }

        public override bool? Test(char c)
        {
            if (Flag == null)
                return char.IsDigit(c);// || c=='-';
            if (c == '.') return pointcnt == 0;
            return char.IsDigit(c);
        }
    }

    public class EParse : Parser.ParseEnvironment
    {
        public EParse()
        {
            base.Operations = new OperationList();
            base.Operations.Add(LogicOperations.And);
            base.Operations.Add(LogicOperations.Or);
            base.Operations.Add(LogicOperations.Not);
            base.Operations.Add(LogicOperations.Xor);
            base.Operations.Add(CoreOperations.Assign);
            base.Operations.Add(CoreOperations.ReAssign);
            base.Operations.Add(BasicOperations.Plus);
            base.Operations.Add(BasicOperations.Minus);
            base.Operations.Add(BasicOperations.Multiply);
            base.Operations.Add(BasicOperations.Divide);
            base.Operations.Add(BasicOperations.Mod);
            base.Operations.Add(BasicOperations.Pow);
            base.Operations.Add(BasicOperations.Equal);
            base.Operations.Add(BasicOperations.Unequal);
            base.Operations.Add(BasicOperations.Bigger);
            base.Operations.Add(BasicOperations.Smaller);
            base.Operations.Add(BasicOperations.NotBigger);
            base.Operations.Add(BasicOperations.NotSmaller);
            base.Operations.Add(ControlStatements.Continue);
            base.Operations.Add(ControlStatements.Break);
            base.Operations.Add(ControlStatements.Return);
            base.Operations.Add(iExpr.Exprs.Core.CoreOperations.Lambda);
            base.VariableChecker = new VariableTokenChecker();
            base.BasicTokenChecker = new BasicTokenChecker();
            base.Constants = new ConstantList();
            Constants.Add(new ConstantToken("true", new ReadOnlyConcreteValue(true)));
            Constants.Add(new ConstantToken("false", new ReadOnlyConcreteValue(false)));
            Constants.Add(new ConstantToken("True", new ReadOnlyConcreteValue(true)));
            Constants.Add(new ConstantToken("False", new ReadOnlyConcreteValue(false)));
            Constants.Add(new ConstantToken("e", new ReadOnlyConcreteValue(System.Math.E)));
            Constants.Add(new ConstantToken("pi", new ReadOnlyConcreteValue(System.Math.PI)));
            Constants.Add(new ConstantToken("null", BuiltinValues.Null));
            Constants.AddFunction(MathOperations.Abs);
            Constants.AddFunction(MathOperations.Sin);
            Constants.AddFunction(MathOperations.Cos);
            Constants.AddFunction(MathOperations.Tan);
            Constants.AddFunction(MathOperations.ArcSin);
            Constants.AddFunction(MathOperations.ArcCos);
            Constants.AddFunction(MathOperations.ArcTan);
            Constants.AddFunction(MathOperations.Ceil);
            Constants.AddFunction(MathOperations.Floor);
            Constants.AddFunction(MathOperations.Round);
            Constants.AddFunction(MathOperations.Sign);
            Constants.AddFunction(MathOperations.Exp);
            Constants.AddFunction(MathOperations.Ln);
            Constants.AddFunction(MathOperations.Log);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.Length);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.List);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.Set);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.Tuple);
            Constants.AddFunction(StatsOperations.Maximum);
            Constants.AddFunction(StatsOperations.Minimum);
            Constants.AddFunction(StatsOperations.Mean);
            Constants.AddFunction(StatsOperations.Total);
            Constants.AddFunction(ControlStatements.For);
            Constants.AddFunction(ControlStatements.ForEach);
            Constants.AddFunction(ControlStatements.If);
            Constants.AddFunction(ControlStatements.While);
            Constants.AddFunction(ControlStatements.DoWhile);
            Constants.AddFunction(CoreOperations.Array);
            Constants.AddFunction(CoreOperations.Func);
            base.BuildOpt();
        }

        public override IValue GetBasicValue(Symbol symbol)
        {
            return new ConcreteValue(double.Parse(symbol.Value));
        }
    }
}
