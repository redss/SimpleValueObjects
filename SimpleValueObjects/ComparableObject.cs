using System;

namespace SimpleValueObjects
{
    // todo: implement IComparable
    // todo: how to force GetHashCode implementation?

    public abstract class ComparableObject<T> : EquitableObject<T>, IComparable<T>, IComparable
        where T : ComparableObject<T>
    {
        protected override bool Equals(T notNullOther)
        {
            return CompareTo(notNullOther) == 0;
        }

        public int CompareTo(object obj)
        {
            if (obj is T || ReferenceEquals(obj, null))
            {
                return CompareToWithNullCheck((T) obj);
            }

            if (!(obj is T))
            {
                throw new ArgumentException(
                    $"Cannot compare object of type {typeof(T).Name} (this type) " +
                    $"to object of type {obj.GetType().Name} (other type).");
            }

            return CompareToWithNullCheck(obj as T);
        }

        int IComparable<T>.CompareTo(T notNullOther)
        {
            return CompareToWithNullCheck(notNullOther);
        }

        private int CompareToWithNullCheck(T other)
        {
            // todo: explain or reference
            return ReferenceEquals(other, null)
                ? 1
                : CompareTo(other);
        }

        protected abstract int CompareTo(T notNullOther);
    }
}