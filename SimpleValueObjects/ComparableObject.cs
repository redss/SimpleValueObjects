using System;

namespace SimpleValueObjects
{
    /// <summary>
    /// Implementations of ComparableObject will compare their order in consistent manner via CompareToNotNull
    /// method, i. e. comparison will yield equivalent results with &lt;, &lt;=, ==, !=, &gt;= and &gt; operators as well as
    /// IEquatable&lt;T&gt;.Equals, object.Equals, IComparable&lt;T&gt;.CompareTo and IComparable.CompareTo methods.
    /// Also, nulls and types are handled properly: every value is greater than null, two nulls are always equal and different types are never equal.
    /// Implementation stil has to handle generating correct hash code.
    /// </summary>
    /// <typeparam name="T">Class implementing ComparableObject</typeparam>
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
        /// All other comparison means (operators, Equals and CompareTo methods) will use this implementation.
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
