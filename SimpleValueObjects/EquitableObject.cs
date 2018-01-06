using System;

namespace SimpleValueObjects
{
    /// <summary>
    /// <para>
    /// Implementations of this class will be equality 
    /// compared using EqualsNotNull implementation.
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
    public abstract class EquitableObject<T> : IEquatable<T>
        where T : EquitableObject<T>
    {
        public static bool operator !=(EquitableObject<T> first, EquitableObject<T> second)
        {
            return !(first == second);
        }

        public static bool operator ==(EquitableObject<T> first, EquitableObject<T> second)
        {
            return ReferenceEquals(first, null)
                ? ReferenceEquals(second, null)
                : first.Equals(second);
        }

        public sealed override bool Equals(object other)
        {
            return Equals(other as T);
        }

        public bool Equals(T other)
        {
            return !ReferenceEquals(other, null) && EqualsNotNull(other);
        }

        /// <summary>
        /// Indicates whether current instance is equal to another instance.
        /// </summary>
        /// <param name="notNullOther">An instance to compare, which is never null.</param>
        protected abstract bool EqualsNotNull(T notNullOther);

        public sealed override int GetHashCode()
        {
            return GenerateHashCode();
        }

        /// <summary>
        /// Computes a hash code for a given instance. If two instances are euqal,
        /// they should also return same hash code. See project documentation
        /// for more information on why and how to implement this method.
        /// </summary>
        protected abstract int GenerateHashCode();
    }
}
