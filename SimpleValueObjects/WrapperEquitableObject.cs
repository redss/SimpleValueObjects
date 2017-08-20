using System;

namespace SimpleValueObjects
{
    public abstract class WrapperEquitableObject<T, TWrapped> : EquitableObject<T>
        where T : WrapperEquitableObject<T, TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperEquitableObject(TWrapped value)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentException(
                    $"{GetType().Name} cannot wrap null value.",
                    nameof(value));
            }

            Value = value;
        }

        public static implicit operator TWrapped(WrapperEquitableObject<T, TWrapped> wrapper)
        {
            return wrapper.Value;
        }

        protected override bool Equals(T notNullOther)
        {
            return Value.Equals(notNullOther.Value);
        }

        public override int GenerateHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}