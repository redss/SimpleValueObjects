﻿using System;

// todo: check all these warnings
#pragma warning disable 660,661,659 // we expect child class to implement GetHashCode

namespace SimpleValueObjects
{
    // todo: how to force GetHashCode implementation?

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

        public override bool Equals(object other)
        {
            return EqualsWithNullCheck(other as T);
        }

        bool IEquatable<T>.Equals(T notNullOther)
        {
            return EqualsWithNullCheck(notNullOther);
        }

        private bool EqualsWithNullCheck(T other)
        {
            return other != null && Equals(other);
        }

        protected abstract bool Equals(T notNullOther);
    }
}
