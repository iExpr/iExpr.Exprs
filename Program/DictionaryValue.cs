using iExpr.Evaluators;
using iExpr.Helpers;
using iExpr.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace iExpr.Exprs.Program
{
    public class DictionaryValue : CollectionValue, IDictionary<IValue,IValue>, IIndexableValue,IAccessibleValue
    {
        private Dictionary<IValue, CollectionItemValue> values = new Dictionary<IValue, CollectionItemValue>();

        public IValue this[IValue key] {
            get => ((IDictionary<IValue, CollectionItemValue>)values)[key];
            set
            {
                ((IDictionary<IValue, CollectionItemValue>)values)[key].Value = value;
            }
        }

        public ICollection<IValue> Keys => ((IDictionary<IValue, CollectionItemValue>)values).Keys;

        public ICollection<IValue> Values => (ICollection<IValue>)((IDictionary<IValue, CollectionItemValue>)values).Values;

        public bool IsReadOnly => ((IDictionary<IValue, CollectionItemValue>)values).IsReadOnly;

        public override int Count => values.Count;

        protected override IEnumerable<CollectionItemValue> _Contents => values.Values;

        public void Add(IValue key, IValue value)
        {
            values.Add(key, new CollectionItemValue(value));
        }

        public void Add(KeyValuePair<IValue, IValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(KeyValuePair<IValue, IValue> item)
        {
            throw new Exceptions.UndefinedExecuteException();
            //return ((IDictionary<IValue, IValue>)values).Contains(new KeyValuePair<IValue, IValue>(item.Key,new CollectionItemValue(item)));
        }

        public override bool Contains(IValue item)
        {
            return ContainsKey(item);
        }

        public bool ContainsKey(IValue key)
        {
            return values.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<IValue, IValue>[] array, int arrayIndex)
        {
            throw new Exceptions.UndefinedExecuteException();
            //((IDictionary<IValue, IValue>)values).CopyTo(array, arrayIndex);
        }

        public override CollectionValue CreateNew()
        {
            return new DictionaryValue();
        }

        public override bool Equals(IExpr other)
        {
            if (!(other is DictionaryValue)) return false;
            return this.ToString() == (other as DictionaryValue).ToString();
        }

        public override bool Equals(IValue other)
        {
            return Equals((IExpr)other);//this.ToString() == other.ToString();
        }

        public IExpr Index(FunctionArgument args, EvalContext cal)
        {
            if (args.Indexs?.Length != 1)
                throw new EvaluateException("The index content is invalid.");
            var ind = cal.GetValue<IValue>(cal.Evaluate(args.Indexs[0]));
            //int ind = ConcreteValueHelper.GetValue<int>(pind);//TODO: Check for null
            return this[ind];
        }

        public bool Remove(IValue key)
        {
            return values.Remove(key);
        }

        public bool Remove(KeyValuePair<IValue, IValue> item)
        {
            throw new Exceptions.UndefinedExecuteException();
            //return ((IDictionary<IValue, IValue>)values).Remove(item);
        }

        public override void Reset(IEnumerable<IValue> vals = null)
        {
            this.Clear();
            if (vals!=null) throw new Exceptions.UndefinedExecuteException();
        }

        public bool TryGetValue(IValue key, out IValue value)
        {
            return ((IDictionary<IValue, IValue>)values).TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<IValue, IValue>> IEnumerable<KeyValuePair<IValue, IValue>>.GetEnumerator()
        {
            foreach(var v in values)
            {
                yield return new KeyValuePair<IValue, IValue>(v.Key,v.Value);
            }
            //return ((IDictionary<IValue, IValue>)values).GetEnumerator();

        }

        public override string ToString()
        {
            if (values == null) return "{}";
            return $"{{{String.Join(" , ", (values).Select(x => $"{x.Key}:{x.Value}"))}}}";
        }

        Dictionary<string, FunctionValue> accessFuncs = new Dictionary<string, FunctionValue>();

        public DictionaryValue()
        {
             accessFuncs.Add("has",ClassValueBuilder.BuildFunction(this.HasKey, "has", 1));
            accessFuncs.Add("remove", ClassValueBuilder.BuildFunction(this.Remove, "remove", 1));
            accessFuncs.Add("add", ClassValueBuilder.BuildFunction(Add, "add", 2));
        }
        
        public object Add(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(2, args);
            OperationHelper.AssertCertainValueThrowIf(args);
            var ov = cal.GetValue<IValue>(args);
            this.Add(ov[0], ov[1]);
            return null;
        }
        
        public object Remove(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(1, args);
            OperationHelper.AssertCertainValueThrowIf(args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.Remove(ov);
        }
        
        public object HasKey(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(1, args);
            OperationHelper.AssertCertainValueThrowIf(args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.ContainsKey(ov);
        }

        public IExpr Access(string id)
        {
            return accessFuncs[id];
            //if (accessFuncs.ContainsKey(id)) 
            
        }
    }
}
