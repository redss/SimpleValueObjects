using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleValueObjects
{
    internal static class TypeExtensions
    {
        public static IEnumerable<FieldInfo> GetInstanceFields(this Type type)
        {
            if (type == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            var instanceFields = type.GetFields(
                BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly);

            var baseInstanceFields = type.BaseType.GetInstanceFields();

            return instanceFields.Concat(baseInstanceFields);
        }
    }
}