using System;

namespace SimpleValueObjects
{
    public abstract class ComparableObject<T> : EquitableObject<T>, IComparable<T>, IComparable
        where T : ComparableObject<T>
    {
        protected override bool IsEqual(T notNullOther)
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
