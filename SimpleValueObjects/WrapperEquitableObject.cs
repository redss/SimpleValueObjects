namespace SimpleValueObjects
{
    /// <summary>
    /// <para>
    /// Implementations of this class will be equality
    /// compared based on a wrapped value equality.
    /// Hash code is computed from wrapped value.
    /// </para>
    /// <para>
    /// Equality comparison will yield equivalent results with == and != 
    /// operators as well as IEquatable&lt;T&gt;.Equals and object.Equals methods.
    /// </para>
    /// <para>
    /// Following rules apply to equality comparison: no value is equal to null,
    /// two nulls are always equal, different types are never equal.
    /// </para>
    /// </summary>
    /// <typeparam name="T">A type implementing this class.</typeparam>
    /// <typeparam name="TWrapped">Type of a wrapped object.</typeparam>
    public abstract class WrapperEquitableObject<T, TWrapped> : EquitableObject<T>
        where T : WrapperEquitableObject<T, TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperEquitableObject(TWrapped value)
        {
            Value = value;
        }

        protected sealed override bool EqualsNotNull(T notNullOther)
        {
            return Equals(Value, notNullOther.Value);
        }

        protected sealed override int GenerateHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "<null>";
        }
    }
}
