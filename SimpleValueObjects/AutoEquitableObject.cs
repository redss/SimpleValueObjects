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
            _fields = typeof(T).GetInstanceFields().ToArray();
        }

        protected sealed override bool IsEqual(T notNullOther)
        {
            return _fields.All(field => FieldValuesAreEqual(field, this, notNullOther));
        }

        private static bool FieldValuesAreEqual(FieldInfo fieldInfo, object first, object second)
        {
            return Equals(fieldInfo.GetValue(first), fieldInfo.GetValue(second));
        }

        protected sealed override int GenerateHashCode()
        {
            var values = _fields.Select(field => field.GetValue(this));

            return HashCodeCalculator.CalculateFromValues(values);
        }
    }
}