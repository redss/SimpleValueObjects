﻿using System;
using System.Linq;
using System.Reflection;

namespace SimpleValueObjects
{
    internal static class TypeExtensions
    {
        // todo: include subtypes' fields
        public static FieldInfo[] GetInstanceFields(this Type type)
        {
            return type.GetRuntimeFields()
                .Where(field => !field.IsStatic)
                .ToArray();
        }
    }
}