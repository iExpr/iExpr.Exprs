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
        protected iExpr.Helpers.ExtendAccessibleValueHelper access = null;

        public virtual EvalContextStartupInfo ContextInfo { get; protected set; }

        void init()
        {
            access = new iExpr.Helpers.ExtendAccessibleValueHelper(false, this);
            access.Add(ClassValueBuilder.BuildFunction(this.HasKey, "has", 1));
            access.Add(ClassValueBuilder.BuildFunction(this.Remove, "remove", 1));
            access.Add(ClassValueBuilder.BuildFunction(Add, "add", 2));
            access.Add(ClassValueBuilder.BuildFunction(Clear, "clear", 0));
        }

        protected Dictionary<IValue, CollectionItemValue> Contents = new Dictionary<IValue, CollectionItemValue>();

        public DictionaryValue()
        {
            init();
        }

        public IValue this[IValue key] {
            get => ((IDictionary<IValue, CollectionItemValue>)Contents)[key];
            set
            {
                ((IDictionary<IValue, CollectionItemValue>)Contents)[key].Value = value;
            }
        }

        public ICollection<IValue> Keys => ((IDictionary<IValue, CollectionItemValue>)Contents).Keys;

        public ICollection<IValue> Values => (ICollection<IValue>)((IDictionary<IValue, CollectionItemValue>)Contents).Values;

        public bool IsReadOnly => ((IDictionary<IValue, CollectionItemValue>)Contents).IsReadOnly;

        public override int Count => Contents.Count;

        protected override IEnumerable<CollectionItemValue> _Contents => Contents.Values;

        public void Add(IValue key, IValue value)
        {
            Contents.Add(key, new CollectionItemValue(value));
        }

        public void Add(KeyValuePair<IValue, IValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Contents.Clear();
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
            return Contents.ContainsKey(key);
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
        

        public IExpr Index(FunctionArgument args, EvalContext cal)
        {
            if (args.Indexs?.Length != 1)
                ExceptionHelper.RaiseIndexFailed(this, args);
            var ind = cal.GetValue<IValue>(cal.Evaluate(args.Indexs[0]));
            //int ind = ConcreteValueHelper.GetValue<int>(pind);//TODO: Check for null
            return this[ind];
        }

        public bool Remove(IValue key)
        {
            return Contents.Remove(key);
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
            return ((IDictionary<IValue, IValue>)Contents).TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<IValue, IValue>> IEnumerable<KeyValuePair<IValue, IValue>>.GetEnumerator()
        {
            foreach(var v in Contents)
            {
                yield return new KeyValuePair<IValue, IValue>(v.Key,v.Value);
            }
            //return ((IDictionary<IValue, IValue>)values).GetEnumerator();

        }

        public override string ToString()
        {
            if (Contents == null) return "{}";
            return $"{{{String.Join(" , ", (Contents).Select(x => $"{x.Key}:{x.Value}"))}}}";
        }
        
        public object Add(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this,2, args);
            OperationHelper.AssertCertainValueThrowIf(this,args);
            var ov = cal.GetValue<IValue>(args);
            this.Add(ov[0], ov[1]);
            return null;
        }
        
        public object Remove(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this,1, args);
            OperationHelper.AssertCertainValueThrowIf(this,args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.Remove(ov);
        }
        
        public object HasKey(FunctionArgument _args, EvalContext cal)
        {
            var args = _args.Arguments;
            OperationHelper.AssertArgsNumberThrowIf(this,1, args);
            OperationHelper.AssertCertainValueThrowIf(this,args);
            var ov = cal.GetValue<IValue>(args[0]);
            return this.ContainsKey(ov);
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
    }
}
