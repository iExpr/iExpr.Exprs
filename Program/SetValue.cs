using iExpr.Evaluators;
using iExpr.Exprs.Core;
using iExpr.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Program
{
    public class SetValue: iExpr.Values.SetValue,IAccessibleValue, IAdditive, ISubtractive, IMultiplicable
    {
        protected iExpr.Helpers.ExtendAccessibleValueHelper access = null;

        void init()
        {
            access = new iExpr.Helpers.ExtendAccessibleValueHelper(false, this);
            access.Add(ClassValueBuilder.BuildFunction(this.Has, "has", 1));
            access.Add(ClassValueBuilder.BuildFunction(this.Remove, "remove", 1));
            access.Add(ClassValueBuilder.BuildFunction(Add, "add", 1));
            access.Add(ClassValueBuilder.BuildFunction(Clear, "clear", 0));
        }

        public SetValue()
        {
            init();
        }

        public SetValue(IEnumerable<IValue> exprs) : base(exprs)
        {
            init();
        }

        public object Add(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this, 1, args);
            OperationHelper.AssertCertainValueThrowIf(this, args);
            var ov = cal.GetValue<IValue>(args[0]);
            this.Add(ov);
            return null;
        }

        public object Remove(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this, 1, args);
            OperationHelper.AssertCertainValueThrowIf(this, args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.Remove(ov);
        }

        public object Has(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this, 1, args);
            OperationHelper.AssertCertainValueThrowIf(this, args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.Contains(ov);
        }

        public object Clear(FunctionArgument _args, EvalContext cal)
        {
            this.Clear();
            return null;
        }

        public IExpr Access(string id)
        {
            return ((IAccessibleValue)access).Access(id);
        }

        public IDictionary<string, IExpr> GetMembers()
        {
            return ((IAccessibleValue)access).GetMembers();
        }

        object IMultiplicable.Multiply(object right)
        {
            List<IValue> ls1 = new List<IValue>(this), ls2 = new List<IValue>();
            var r = (Values.IEnumerableValue)OperationHelper.GetValue((IExpr)right);

            foreach (var v in r)
            {
                ls2.Add(v);
            }

            List<IValue> ls = new List<IValue>();
            foreach (var v in ls1)
            {
                foreach (var v2 in ls2)
                {
                    ls.Add(new TupleValue(new IValue[] { v, v2 }));
                }
            }
            return new SetValue(ls);
        }

        object ISubtractive.Subtract(object right)
        {
            List<IValue> ls = new List<IValue>(this);
            var r = (Values.IEnumerableValue)OperationHelper.GetValue((IExpr)right);
            foreach (var v in r)
            {
                ls.Remove(v);
            }
            return new SetValue(ls);
        }

        object ISubtractive.Negtive()
        {
            return new SetValue();
        }

        object IAdditive.Add(object right)
        {
            List<IValue> ls = new List<IValue>(this);
            var r = (Values.IEnumerableValue)OperationHelper.GetValue((IExpr)right);
            foreach (var v in r)
            {
                ls.Add(v);
            }
            return new SetValue(ls);
        }
    }
}
