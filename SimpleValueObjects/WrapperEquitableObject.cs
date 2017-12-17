namespace SimpleValueObjects
{
    // todo: consider adding null check
    // todo: consider adding implicit casts

    public abstract class WrapperEquitableObject<T, TWrapped> : EquitableObject<T>
        where T : WrapperEquitableObject<T, TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperEquitableObject(TWrapped value)
        {
            Value = value;
        }

        protected override bool EqualsNotNull(T notNullOther)
        {
            return Equals(Value, notNullOther.Value);
        }

        protected override int GenerateHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "<null>";
        }
    }
}
