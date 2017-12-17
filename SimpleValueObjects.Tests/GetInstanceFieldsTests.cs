using System.Linq;
using FluentAssertions;
using NUnit.Framework;

#pragma warning disable 169

// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Local

namespace SimpleValueObjects.Tests
{
    public class GetInstanceFieldsTests
    {
        [Test]
        public void gets_only_instance_fields_from_a_class()
        {
            typeof(SomeClass)
                .GetInstanceFields()
                .Select(field => field.Name)
                .Should()
                .BeEquivalentTo(SomeClass.InstanceFieldNames);
        }

        [Test]
        public void also_gets_instance_fields_from_all_super_classes()
        {
            typeof(SomeSubSubClass)
                .GetInstanceFields()
                .Select(field => field.Name)
                .Should()
                .BeEquivalentTo(SomeClass.InstanceFieldNames
                    .Concat(SomeSubClass.SubInstanceFieldNames)
                    .Concat(SomeSubSubClass.SubSubInstanceFieldNames));
        }

        class SomeSubSubClass : SomeSubClass
        {
            public const int SubSubPublicConstField = 0;
            private const int SubSubPrivateConstField = 0;

            public static int SubSubPublicStaticField = 0;
            private static int SubSubPrivateStaticField = 0;

            public static readonly int SubSubPublicStaticReadonlyField = 0;
            private static readonly int SubSubPrivateStaticReadonlyField = 0;

            public int SubSubPublicInstanceField = 0;
            private int SubSubPrivateInstanceField = 0;

            public readonly int SubSubPublicReadonlyField = 0;
            private readonly int SubSubPrivateReadonlyField = 0;

            public static string[] SubSubInstanceFieldNames => new[]
            {
                nameof(SubSubPublicInstanceField),
                nameof(SubSubPrivateInstanceField),
                nameof(SubSubPublicReadonlyField),
                nameof(SubSubPrivateReadonlyField)
            };
        }

        class SomeSubClass : SomeClass
        {
            public const int SubPublicConstField = 0;
            private const int SubPrivateConstField = 0;

            public static int SubPublicStaticField = 0;
            private static int SubPrivateStaticField = 0;

            public static readonly int SubPublicStaticReadonlyField = 0;
            private static readonly int SubPrivateStaticReadonlyField = 0;

            public int SubPublicInstanceField = 0;
            private int SubPrivateInstanceField = 0;

            public readonly int SubPublicReadonlyField = 0;
            private readonly int SubPrivateReadonlyField = 0;

            public static string[] SubInstanceFieldNames => new[]
            {
                nameof(SubPublicInstanceField),
                nameof(SubPrivateInstanceField),
                nameof(SubPublicReadonlyField),
                nameof(SubPrivateReadonlyField)
            };
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