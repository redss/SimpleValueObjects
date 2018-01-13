using FluentAssertions;
using NUnit.Framework;

// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ConditionIsAlwaysTrueOrFalse - since we overload == and != operators, it's not so sure

namespace SimpleValueObjects.Tests
{
    public class ValueObjectTests
    {
        [Test]
        public void identical_objects_are_always_equal_equal()
        {
            var first = new SomeValueObject(1);
            var second = new SomeValueObject(1);

            ShouldBeEqualAccordingToEqualityOperators(first, second);
            ShouldBeEqualAccordingToEqualsMethods(first, second);
            ShouldBeEqualAccordingToEqualsMethods(second, first);
        }

        [Test]
        public void different_objects_are_never_equal()
        {
            var first = new SomeValueObject(1);
            var second = new SomeValueObject(2);

            ShouldNotBeEqualAccordingToEqualityOperators(first, second);
            ShouldNotBeEqualAccordingToEqualsMethods(first, second);
            ShouldNotBeEqualAccordingToEqualsMethods(second, first);
        }

        [Test]
        public void any_value_is_never_equal_to_null()
        {
            var something = new SomeValueObject(1);
            SomeValueObject nothing = null;

            ShouldNotBeEqualAccordingToEqualityOperators(something, nothing);
            ShouldNotBeEqualAccordingToEqualsMethods(something, nothing);
        }

        [Test]
        public void two_nulls_are_always_equal()
        {
            SomeValueObject first = null;
            SomeValueObject second = null;

            ShouldBeEqualAccordingToEqualityOperators(first, second);
            ShouldBeEqualAccordingToEqualityOperators(second, first);
        }

        [Test]
        public void any_value_is_always_equal_to_itself()
        {
            var something = new SomeValueObject(1);

            ShouldBeEqualAccordingToEqualityOperators(something, something);
            ShouldBeEqualAccordingToEqualsMethods(something, something);
        }

        [Test]
        public void two_different_types_are_never_equal()
        {
            var something = new SomeValueObject(1);

            something.Equals("some string").Should().BeFalse();
        }

        private static void ShouldBeEqualAccordingToEqualityOperators(SomeValueObject first, SomeValueObject second)
        {
            (first == second).Should().BeTrue();
            (first != second).Should().BeFalse();
            (second == first).Should().BeTrue();
            (second != first).Should().BeFalse();
        }

        private static void ShouldNotBeEqualAccordingToEqualityOperators(SomeValueObject first, SomeValueObject second)
        {
            (first == second).Should().BeFalse();
            (first != second).Should().BeTrue();
            (second == first).Should().BeFalse();
            (second != first).Should().BeTrue();
        }

        private static void ShouldBeEqualAccordingToEqualsMethods(SomeValueObject first, SomeValueObject second)
        {
            first.Equals((object)second).Should().BeTrue();
            first.Equals(second).Should().BeTrue();
        }

        private static void ShouldNotBeEqualAccordingToEqualsMethods(SomeValueObject first, SomeValueObject second)
        {
            first.Equals((object)second).Should().BeFalse();
            first.Equals(second).Should().BeFalse();
        }

        class SomeValueObject : ValueObject<SomeValueObject>
        {
            private readonly int _value;

            public SomeValueObject(int value) => _value = value;

            protected override bool EqualsNotNull(SomeValueObject notNullOther) => _value == notNullOther._value;

            protected override int GenerateHashCode() => _value;
        }
    }
}