using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValueObjects
{
    // todo: test and make public 
    // todo: consider approach using generic values and comparison builder

    internal abstract class AutoComparableObject<T> : ComparableObject<T>
        where T : AutoComparableObject<T>
    {
        protected abstract IEnumerable<IComparable> GetValuesForComparison();

        protected sealed override int CompareToNotNull(T notNullOther)
        {
            var thisValues = GetValuesForComparison().ToArray();
            var otherValues = notNullOther.GetValuesForComparison().ToArray();

            if (thisValues.Length != otherValues.Length)
            {
                throw new InvalidOperationException(
                    $"Cannot compare instances of {GetType().Name}, " +
                    $"because they returned different number of arguments: " +
                    $"{thisValues.Length } and {otherValues.Length}. " +
                    $"Probably {nameof(GetValuesForComparison)} method is not implemented correctly.");
            }

            return thisValues
                .Zip(otherValues, CompareValues)
                .FirstOrDefault(result => result != 0);
        }

        private static int CompareValues(IComparable thisValue, IComparable otherValue)
        {
            if (thisValue != null)
            {
                return thisValue.CompareTo(otherValue);
            }

            return otherValue == null ? 0 : -1;
        }

        protected override int GenerateHashCode()
        {
            var values = GetValuesForComparison();

            return HashCodeCalculator.CalculateFromValues(values);
        }
    }
}
