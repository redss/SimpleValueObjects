using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType - this is our intention

namespace SimpleValueObjects
{
    /// <summary>
    /// Implementations of AutoEquitableObject will compute their equality based on their field value comparison.
    /// 
    /// Equality comparison will yield equivalent results with == and != operators as well as
    /// IEquatable&lt;T&gt;.Equals and object.Equals methods. 
    /// 
    /// Also, nulls and types are handled properly: no value is equal to null,
    /// two nulls are always equal and different types are never equal.
    /// 
    /// Generating hash code is also implemented.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AutoEquitableObject<T> : EquitableObject<T>
        where T : AutoEquitableObject<T>
    {
        private static readonly FieldInfo[] _fields;

        static AutoEquitableObject()
        {
            _fields = typeof(T).GetInstanceFields().ToArray();
        }

        protected sealed override bool EqualsNotNull(T notNullOther)
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