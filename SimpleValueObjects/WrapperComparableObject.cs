using System;

namespace SimpleValueObjects
{
    public abstract class WrapperComparableObject<T, TWrapped> : ComparableObject<T>
        where T : WrapperComparableObject<T, TWrapped>
        where TWrapped : IComparable<TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperComparableObject(TWrapped value)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentException(
                    $"{GetType().Name} cannot wrap null value.",
                    nameof(value));
            }

            Value = value;
        }

        public static implicit operator TWrapped(WrapperComparableObject<T, TWrapped> wrapper)
        {
            return wrapper.Value;
        }

        protected override int CompareTo(T notNullOther)
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