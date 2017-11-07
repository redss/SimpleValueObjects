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

        public static int Compare(ComparableObject<T> first, ComparableObject<T> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
            {
                return 0;
            }

            if (ReferenceEquals(first, null))
            {
                return -1;
            }

            if (ReferenceEquals(second, null))
            {
                return 1;
            }

            // todo: hmm
            return first.CompareToNotNull((T)second);
        }
    }
}
