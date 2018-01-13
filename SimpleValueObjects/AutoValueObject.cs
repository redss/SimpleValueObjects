using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType - this is our intention

namespace SimpleValueObjects
{
    /// <summary>
    /// <para>
    /// Implementations of this class will be equality
    /// compared based on equality of their field values.
    /// Hash code is also computed based on field values.
    /// </para>
    /// <para>
    /// Equality comparison will yield equivalent results with == and != 
    /// operators as well as IEquatable&lt;T&gt;.Equals and object.Equals methods.
    /// </para>
    /// <para>
    /// Following rules apply to equality comparison: no value is equal to null,
    /// two nulls are always equal, different types are never equal.
    /// </para>
    /// </summary>
    /// <typeparam name="T">A type implementing this class.</typeparam>
    public abstract class AutoValueObject<T> : ValueObject<T>
        where T : AutoValueObject<T>
    {
        private static readonly FieldInfo[] _fields = typeof(T).GetInstanceFields().ToArray();

        protected sealed override bool EqualsNotNull(T notNullOther)
        {
            return _fields.All(field => FieldValuesAreEqual(field, first: this, second: notNullOther));
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