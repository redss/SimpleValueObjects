using System;

namespace SimpleValueObjects
{
    public abstract class ComparableObject<T> : EquitableObject<T>, IComparable<T>, IComparable
        where T : ComparableObject<T>
    {
        protected override bool Equals(T notNullOther)
        {
            return CompareTo(notNullOther) == 0;
        }

        int IComparable.CompareTo(object other)
        {
            if (other is T || ReferenceEquals(other, null))
            {
                return CompareToWithNullCheck((T) other);
            }

            throw new ArgumentException(
                $"Cannot compare object of type {typeof(T).Name} (this type) " +
                $"to object of type {other.GetType().Name} (other type).");
        }

        int IComparable<T>.CompareTo(T notNullOther)
        {
            return CompareToWithNullCheck(notNullOther);
        }

        private int CompareToWithNullCheck(T other)
        {
            // todo: explain or reference
            // todo: is reference equals necessary (won't just == do)?
            return ReferenceEquals(other, null)
                ? 1
                : CompareTo(other);
        }

        protected abstract int CompareTo(T notNullOther);
    }
}
