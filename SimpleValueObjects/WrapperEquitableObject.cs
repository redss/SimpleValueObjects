using System;

namespace SimpleValueObjects
{
    public abstract class WrapperEquitableObject<T, TWrapped> : EquitableObject<T>
        where T : WrapperEquitableObject<T, TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperEquitableObject(TWrapped value)
        {
            if (value == null)
            {
                throw new ArgumentException(
                    $"Cannot wrap null value in WrapperComparableObject of {typeof(TWrapped).Name}.",
                    nameof(value));
            }

            Value = value;
        }

        public static implicit operator TWrapped(WrapperEquitableObject<T, TWrapped> wrapper)
        {
            return wrapper.Value;
        }

        protected override bool IsEqual(T notNullOther)
        {
            return Value.Equals(notNullOther.Value);
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