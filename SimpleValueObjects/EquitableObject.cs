using System;

namespace SimpleValueObjects
{
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
            return EqualsWithNullCheck(other as T);
        }

        public bool Equals(T other)
        {
            return EqualsWithNullCheck(other);
        }

        private bool EqualsWithNullCheck(T other)
        {
            return !ReferenceEquals(other, null) && IsEqual(other);
        }

        protected abstract bool IsEqual(T notNullOther);

        // todo: is enforcing GetHashCode implementation necessary?

        public sealed override int GetHashCode()
        {
            return GenerateHashCode();
        }

        protected abstract int GenerateHashCode();
    }
}
