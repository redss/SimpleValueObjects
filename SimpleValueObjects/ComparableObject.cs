using System;

namespace SimpleValueObjects
{
    /// <summary>
    /// <para>
    /// Implementations of this class will be compared and 
    /// equality compared using CompareToNotNull implementation.
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
    public abstract class ComparableObject<T> : EquitableObject<T>, IComparable<T>, IComparable
        where T : ComparableObject<T>
    {
        protected override bool EqualsNotNull(T notNullOther)
        {
            return CompareToNotNull(notNullOther) == 0;
        }

        int IComparable.CompareTo(object other)
        {
            if (other is T || ReferenceEquals(other, null))
            {
                return CompareTo((T)other);
            }

            throw new ArgumentException(
                $"Cannot compare object of type {typeof(T).Name} (this type) " +
                $"with object of type {other.GetType().Name} (other type) using CompareTo(object).");
        }

        public int CompareTo(T other)
        {
            return ReferenceEquals(other, null)
                ? 1
                : CompareToNotNull(other);
        }

        /// <summary>
        /// Indicates whether current instance whether the current instance precedes, follows,
        /// or occurs in the same position in the sort order as another instance.
        /// </summary>
        /// <param name="notNullOther">An instance to compare, which is never null.</param>
        protected abstract int CompareToNotNull(T notNullOther);

        public static bool operator >(ComparableObject<T> first, ComparableObject<T> second)
        {
            return Compare(first, second) > 0;
        }

        public static bool operator >=(ComparableObject<T> first, ComparableObject<T> second)
        {
            return Compare(first, second) >= 0;
        }

        public static bool operator <=(ComparableObject<T> first, ComparableObject<T> second)
        {
            return Compare(first, second) <= 0;
        }

        public static bool operator <(ComparableObject<T> first, ComparableObject<T> second)
        {
            return Compare(first, second) < 0;
        }

        public static implicit operator T(ComparableObject<T> comparableObject)
        {
            return (T) comparableObject;
        }

        private static int Compare(T first, T second)
        {
            if (first != null)
            {
                return first.CompareTo(second);
            }

            return second == null ? 0 : -1;
        }
    }
}
