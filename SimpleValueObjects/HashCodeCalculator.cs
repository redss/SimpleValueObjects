using System.Collections.Generic;
using System.Linq;

namespace SimpleValueObjects
{
    // Implementation is based on https://stackoverflow.com/a/263416
    // and https://stackoverflow.com/a/2816747

    internal static class HashCodeCalculator
    {
        public static int CalculateFromValues(IEnumerable<object> values)
        {
            return values.Aggregate(
                seed: HashCodePrime,
                func: IncludeValueInHashCode);
        }

        private static int IncludeValueInHashCode(int hashCode, object value)
        {
            unchecked
            {
                var fieldsHashCode = value?.GetHashCode() ?? 0;

                return hashCode * HashCodePrime + fieldsHashCode;
            }
        }

        private const int HashCodePrime = 92821;
    }
}