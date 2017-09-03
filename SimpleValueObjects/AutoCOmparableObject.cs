using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValueObjects
{
    public abstract class AutoComparableObject<T> : ComparableObject<T>
        where T : AutoComparableObject<T>
    {
        protected abstract IEnumerable<IComparable> GetValuesForComparison();

        protected sealed override int CompareTo(T notNullOther)
        {
            var valuesForComparison = GetValuesForComparison().ToArray();
            var otherValuesForComparison = notNullOther.GetValuesForComparison().ToArray();

            // todo: error when number of values differ

            if (valuesForComparison.Length != otherValuesForComparison.Length)
            {
                throw new InvalidOperationException(
                    $"Cannot compare objects of type {GetType().Name}, " +
                    $"because two different objects returned different number of arguments: " +
                    $"{valuesForComparison.Length } and {otherValuesForComparison.Length}. " +
                    $"Probably {nameof(GetValuesForComparison)} method is not implemented correctly.");
            }

            for (var i = 0; i < valuesForComparison.Length; i++)
            {
                var result = valuesForComparison[i].CompareTo(otherValuesForComparison[i]);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        protected sealed override int GenerateHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
