using System;

namespace SimpleValueObjects
{
    // todo: consider adding null check
    // todo: consider adding implicit casts

    internal abstract class WrapperComparableObject<T, TWrapped> : ComparableObject<T>
        where T : WrapperComparableObject<T, TWrapped>
        where TWrapped : IComparable<TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperComparableObject(TWrapped value)
        {
            Value = value;
        }

        public static implicit operator TWrapped(WrapperComparableObject<T, TWrapped> wrapper)
        {
            return wrapper.Value;
        }

        protected override int CompareToNotNull(T notNullOther)
        {
            return Value.CompareTo(notNullOther);
        }

        protected override int GenerateHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}