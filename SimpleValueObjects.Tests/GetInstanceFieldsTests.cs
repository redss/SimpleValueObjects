using System.Linq;
using FluentAssertions;
using NUnit.Framework;

#pragma warning disable 169

// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local

namespace SimpleValueObjects.Tests
{
    public class GetInstanceFieldsTests
    {
        [Test]
        public void can_get_only_instance_fields_from_a_class()
        {
            typeof(SomeClass)
                .GetInstanceFields()
                .Select(field => field.Name)
                .Should()
                .BeEquivalentTo(SomeClass.InstanceFieldNames);
        }

        class SomeClass
        {
            public const int PublicConstField = 0;
            private const int PrivateConstField = 0;

            public static int PublicStaticField = 0;
            private static int PrivateStaticField = 0;

            public static readonly int PublicStaticReadonlyField = 0;
            private static readonly int PrivateStaticReadonlyField = 0;

            public int PublicInstanceField = 0;
            private int PrivateInstanceField = 0;

            public readonly int PublicReadonlyField = 0;
            private readonly int PrivateReadonlyField = 0;

            public static string[] InstanceFieldNames => new[]
            {
                nameof(PublicInstanceField),
                nameof(PrivateInstanceField),
                nameof(PublicReadonlyField),
                nameof(PrivateReadonlyField)
            };
        }
    }
}