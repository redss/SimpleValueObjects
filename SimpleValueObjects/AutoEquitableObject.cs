using System.Linq;
using System.Reflection;

namespace SimpleValueObjects
{
    // todo: think about better name

    public abstract class AutoEquitableObject<T> : EquitableObject<T>
        where T : AutoEquitableObject<T>
    {
        protected override bool Equals(T notNullOther)
        {
            return GetType()
                .GetRuntimeFields() // todo: this method might not cut it
                .All(field => FieldValuesAreEqual(field, this, notNullOther));
        }

        private static bool FieldValuesAreEqual(FieldInfo fieldInfo, object first, object second)
        {
            var thisValue = fieldInfo.GetValue(first);
            var otherValue = fieldInfo.GetValue(second);

            // todo: double check this approach
            return !ReferenceEquals(thisValue, null) && thisValue.Equals(otherValue);
        }

        public override int GenerateHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}