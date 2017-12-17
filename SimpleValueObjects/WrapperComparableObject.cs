using System;
using System.Collections.Generic;

namespace SimpleValueObjects
{
    public abstract class WrapperComparableObject<T, TWrapped> : ComparableObject<T>
        where T : WrapperComparableObject<T, TWrapped>
        where TWrapped : IComparable<TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperComparableObject(TWrapped value)
        {
            Value = value;
        }

        protected override int CompareToNotNull(T notNullOther)
        {
            return Comparer<TWrapped>.Default.Compare(Value, notNullOther.Value);
        }

        protected override int GenerateHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}