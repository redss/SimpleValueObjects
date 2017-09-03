using System.Linq;
using System.Reflection;

namespace SimpleValueObjects
{
    // todo: think about better name

    public abstract class AutoEquitableObject<T> : EquitableObject<T>
        where T : AutoEquitableObject<T>
    {
        protected sealed override bool IsEqual(T notNullOther)
        {
            return GetType()
                .GetRuntimeFields()
                .Where(field => !field.IsStatic)
                .Where(field => !field.IsLiteral)
                .All(field => FieldValuesAreEqual(field, this, notNullOther));
        }

        private static bool FieldValuesAreEqual(FieldInfo fieldInfo, object first, object second)
        {
            var thisValue = fieldInfo.GetValue(first);
            var otherValue = fieldInfo.GetValue(second);

            // todo: double check this approach
            return !ReferenceEquals(thisValue, null) && thisValue.Equals(otherValue);
        }

        protected override int GenerateHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}