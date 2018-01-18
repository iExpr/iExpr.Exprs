using iExpr.Evaluators;
using iExpr.Exceptions;
using iExpr.Exprs.Core;
using iExpr.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Program
{
    public class TupleValue : iExpr.Values.TupleValue,IAccessibleValue, IAdditive, ISubtractive, IMultiplicable
    {
        protected iExpr.Helpers.ExtendAccessibleValueHelper access = null;

        void init()
        {
            access = new iExpr.Helpers.ExtendAccessibleValueHelper(false, this);
            access.Add(ClassValueBuilder.BuildFunction(this.IndexOf, "index", 1));
            access.Add(ClassValueBuilder.BuildFunction(this.Has, "has", 1));
        }

        public TupleValue()
        {
            init();
        }

        public override IValue this[int index]
        {
            get => base[index >= 0 ? index : Count + index];
            set => base[index >= 0 ? index : Count + index] = value;
        }

        public TupleValue(IEnumerable<IValue> exprs) : base(exprs)
        {
        }

        List<IValue> slice(int l, int r)
        {
            int k = Math.Sign(r - l);
            List<IValue> res = new List<IValue>();
            for (int i = l; i != r; i += k) res.Add(this[i]);
            res.Add(this[r]);
            return res;
        }

        public override IExpr Index(FunctionArgument args, EvalContext cal)
        {
            if (args.Indexs == null)
                ExceptionHelper.RaiseIndexFailed(this, args);

            switch (args.Indexs.Length)
            {
                case 1:
                    var ind = cal.GetValue<int>(cal.Evaluate(args.Indexs[0]));
                    return this[ind];
                case 2:
                    var s = cal.GetValue<int>(cal.Evaluate(args.Indexs[0]), cal.Evaluate(args.Indexs[1]));
                    return new TupleValue(this.slice(s[0], s[1]));
                default:
                    ExceptionHelper.RaiseIndexFailed(this, args);
                    return default;
            }
        }

        public object Has(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this, 1, args);
            OperationHelper.AssertCertainValueThrowIf(this, args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.Contains(ov);
        }

        int IndexOf(IValue item)
        {
            for(int i=0;i<Contents.Length;i++)
            {
                if (Contents[i].Equals(item)) return i;
            }
            return -1;
        }

        public object IndexOf(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this, 1, args);
            OperationHelper.AssertCertainValueThrowIf(this, args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.IndexOf(ov);
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
            return new TupleValue(ls);
        }

        object ISubtractive.Subtract(object right)
        {
            List<IValue> ls = new List<IValue>(this);
            var r = (Values.IEnumerableValue)OperationHelper.GetValue((IExpr)right);
            foreach (var v in r)
            {
                ls.Remove(v);
            }
            return new TupleValue(ls);
        }

        object ISubtractive.Negtive()
        {
            return new TupleValue();
        }

        object IAdditive.Add(object right)
        {
            List<IValue> ls = new List<IValue>(this);
            var r = (Values.IEnumerableValue)OperationHelper.GetValue((IExpr)right);
            foreach (var v in r)
            {
                ls.Add(v);
            }
            return new TupleValue(ls);
        }
    }
}
