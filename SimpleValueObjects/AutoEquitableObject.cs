using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType - this is our intention

namespace SimpleValueObjects
{
    public abstract class AutoEquitableObject<T> : EquitableObject<T>
        where T : AutoEquitableObject<T>
    {
        private static readonly FieldInfo[] _fields;

        static AutoEquitableObject()
        {
            _fields = typeof(T).GetInstanceFields();
        }

        protected sealed override bool IsEqual(T notNullOther)
        {
            return _fields.All(field => FieldValuesAreEqual(field, this, notNullOther));
        }

        private static bool FieldValuesAreEqual(FieldInfo fieldInfo, object first, object second)
        {
            return Equals(fieldInfo.GetValue(first), fieldInfo.GetValue(second));
        }

        // GetHashCode implementation is based on https://stackoverflow.com/a/263416
        // and https://stackoverflow.com/a/2816747

        private const int HashCodePrime = 92821;

        protected sealed override int GenerateHashCode()
        {
            return _fields.Aggregate(
                seed: HashCodePrime,
                func: IncludeFieldInHashCode);
        }

        private int IncludeFieldInHashCode(int hashCode, FieldInfo field)
        {
            unchecked
            {
                var fieldsHashCode = field.GetValue(this)?.GetHashCode() ?? 0;

                return hashCode * HashCodePrime + fieldsHashCode;
            }
        }
    }
}