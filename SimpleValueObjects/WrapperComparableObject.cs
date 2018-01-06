using System;
using System.Collections.Generic;

namespace SimpleValueObjects
{
    // todo: mention hash code in doc

    /// <summary>
    /// <para>
    /// Implementations of this class will be compared 
    /// based on a wrapped value comparison.
    /// Hash code is computed from wrapped value.
    /// </para>
    /// <para>
    /// Comparison will yield equivalent results with &lt;, &lt;=, ==, !=, &gt;= and &gt;
    /// operators as well as IEquatable&lt;T&gt;.Equals, object.Equals,
    /// IComparable&lt;T&gt;.CompareTo and IComparable.CompareTo methods.
    /// </para>
    /// <para>
    /// Following rules apply to equality comparison: no value is equal to null,
    /// two nulls are always equal, different types are never equal.
    /// </para>
    /// <para>
    /// Following rules apply to comparison: every value is greater than 
    /// null and comparing different types will throw an exception.
    /// </para>
    /// </summary>
    /// <typeparam name="T">A type implementing this class.</typeparam>
    /// <typeparam name="TWrapped">Type of a wrapped object. It has to implement IComparable&lt;T&gt;.</typeparam>
    public abstract class WrapperComparableObject<T, TWrapped> : ComparableObject<T>
        where T : WrapperComparableObject<T, TWrapped>
        where TWrapped : IComparable<TWrapped>
    {
        public TWrapped Value { get; }

        protected WrapperComparableObject(TWrapped value)
        {
            Value = value;
        }

        protected sealed override int CompareToNotNull(T notNullOther)
        {
            return Comparer<TWrapped>.Default.Compare(Value, notNullOther.Value);
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