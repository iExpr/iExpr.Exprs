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
        //bool isneg = false;

        public override void Clear()
        {
            base.Clear();pointcnt = 0;
            //isneg = false;
        }

        public override bool? Append(char c)
        {
            var res=base.Append(c);
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
            base.Operations.Add(Operators.Plus);
            base.Operations.Add(Operators.Minus);
            base.Operations.Add(Operators.Multiply);
            base.Operations.Add(Operators.Divide);
            base.Operations.Add(Operators.Mod);
            base.Operations.Add(Operators.Pow);
            base.Operations.Add(Operators.Equal);
            base.Operations.Add(Operators.Unequal);
            base.Operations.Add(Operators.Bigger);
            base.Operations.Add(Operators.Smaller);
            base.Operations.Add(Operators.NotBigger);
            base.Operations.Add(Operators.NotSmaller);
            base.Operations.Add(Operators.Factorial);
            base.Operations.Add(Operators.Not);
            base.Operations.Add(iExpr.Exprs.Core.CoreOperations.Lambda);
            base.VariableChecker = new VariableTokenChecker();
            base.BasicTokenChecker = new BasicTokenChecker();
            base.Constants = new ConstantList();
            Constants.Add(new ConstantToken("e", new ReadOnlyConcreteValue(System.Math.E)));
            Constants.Add(new ConstantToken("pi", new ReadOnlyConcreteValue(System.Math.PI)));
            Constants.AddFunction(Operators.Abs);
            Constants.AddFunction(Operators.Sin);
            Constants.AddFunction(Operators.Cos);
            Constants.AddFunction(Operators.Tan);
            Constants.AddFunction(Operators.ArcSin);
            Constants.AddFunction(Operators.ArcCos);
            Constants.AddFunction(Operators.ArcTan);
            Constants.AddFunction(Operators.Ceil);
            Constants.AddFunction(Operators.Floor);
            Constants.AddFunction(Operators.Round);
            Constants.AddFunction(Operators.Sign);
            Constants.AddFunction(Operators.Exp);
            Constants.AddFunction(Operators.Ln);
            Constants.AddFunction(Operators.Log);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.Length);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.List);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.Set);
            Constants.AddFunction(iExpr.Exprs.Core.CoreOperations.Tuple);
            Constants.AddFunction(iExpr.Exprs.Math.StatsOperations.Maximum);
            Constants.AddFunction(iExpr.Exprs.Math.StatsOperations.Minimum);
            Constants.AddFunction(iExpr.Exprs.Math.StatsOperations.Mean);
            Constants.AddFunction(iExpr.Exprs.Math.StatsOperations.Total);
            Constants.AddFunction(iExpr.Exprs.Math.StatsOperations.Sum);
            Constants.AddFunction(iExpr.Exprs.Math.StatsOperations.Product);
            Constants.AddFunction(iExpr.Exprs.Math.NumberTheoryOperations.Gcd);
            Constants.AddFunction(iExpr.Exprs.Math.NumberTheoryOperations.Lcm);
            Constants.AddFunction(iExpr.Exprs.Math.SetOperations.Cap);
            Constants.AddFunction(iExpr.Exprs.Math.SetOperations.Cup);
            Constants.AddFunction(iExpr.Exprs.Math.SetOperations.Dif);
            base.BuildOpt();
        }

        public override IValue GetBasicValue(Symbol symbol)
        {
            return new ConcreteValue(double.Parse(symbol.Value));
        }
    }
}
