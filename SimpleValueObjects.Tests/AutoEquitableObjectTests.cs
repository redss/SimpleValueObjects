using FluentAssertions;
using NUnit.Framework;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable MemberCanBePrivate.Local

namespace SimpleValueObjects.Tests
{
    public class AutoEquitableObjectTests
    {
        [Datapoints]
        public readonly int[] IntDatapoints =
        {
            int.MinValue,
            -123,
            0,
            123,
            int.MaxValue
        };

        [Datapoints]
        public readonly string[] StringDatapoints =
        {
            null,
            "",
            "hello",
            "world"
        };

        [Theory]
        public void objects_are_equal_and_have_same_hash_code_when_their_fields_are_equal(
            int someInt, string someString, bool someBool)
        {
            var first = new SomeValueObject(someInt, someString, someBool);
            var second = new SomeValueObject(someInt, someString, someBool);

            first.Should().Be(second);
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Theory]
        public void objects_are_not_equal_and_have_different_hash_codes_when_their_fields_are_not_equal(
            int firstInt, string firstString, bool firstBool,
            int secondInt, string secondString, bool secondBool)
        {
            Assume.That(firstInt != secondInt
                || firstString != secondString
                || firstBool != secondBool);

            var first = new SomeValueObject(firstInt, firstString, firstBool);
            var second = new SomeValueObject(secondInt, secondString, secondBool);

            first.Should().NotBe(second);

            // since there is a small chance of hash code collision,
            // we're kind of hoping for the best here
            first.GetHashCode().Should().NotBe(second.GetHashCode());
        }

        class SomeValueObject : AutoEquitableObject<SomeValueObject>
        {
            public int SomeInt { get; }
            public string SomeString { get; }
            public bool SomeBool { get; }

            public SomeValueObject(int someInt, string someString, bool someBool)
            {
                SomeInt = someInt;
                SomeString = someString;
                SomeBool = someBool;
            }
        }

        [Test]
        public void two_instances_of_object_without_fields_are_always_equal_and_have_same_hash_code()
        {
            var first = new ValueObjectWithNoFields();
            var second = new ValueObjectWithNoFields();

            first.Should().Be(second);
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        class ValueObjectWithNoFields : AutoEquitableObject<ValueObjectWithNoFields>
        {
        }
    }
}