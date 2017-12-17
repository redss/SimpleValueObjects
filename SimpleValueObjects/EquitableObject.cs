using System;

namespace SimpleValueObjects
{
    /// <summary>
    /// Implementations of EquitableObject will compare their equality in consistent manner based on EqualsNotNull implementation.
    ///  
    /// Equality comparison will yield equivalent results with == and != operators as well as
    /// IEquatable&lt;T&gt;.Equals and object.Equals methods. 
    /// 
    /// Also, nulls and types are handled properly: no value is equal to null,
    /// two nulls are always equal and different types are never equal.
    /// 
    /// Implementation stil has to handle generating correct hash code.
    /// </summary>
    /// <typeparam name="T">Class implementing EquitableObject</typeparam>
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
        /// Indicates whether current instance is equal to another, not null instance.
        /// All other equality comparison means (operators and Equals methods) will use this implementation.
        /// </summary>
        /// <param name="notNullOther">An instance to compare, which is never null.</param>
        protected abstract bool EqualsNotNull(T notNullOther);

        public sealed override int GetHashCode()
        {
            return GenerateHashCode();
        }

        /// <summary>
        /// Computes a hash code for a given instance.
        /// See following for information on how to compute one:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=netframework-4.7.1#Remarks
        /// </summary>
        protected abstract int GenerateHashCode();
    }
}
