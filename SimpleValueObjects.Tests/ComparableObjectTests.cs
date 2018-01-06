using System;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable ConditionIsAlwaysTrueOrFalse - since we overload == and != operators, it's not so sure

namespace SimpleValueObjects.Tests
{
    // based on: https://msdn.microsoft.com/en-us/library/system.icomparable.compareto.aspx

    public class ComparableObjectTests
    {
        [Test]
        public void equal_objects_compare_equal()
        {
            var first = new SomeComparableObject(1);
            var second = new SomeComparableObject(1);

            ShouldBeEqualAccordingToCompareToMethods(first, second);
            ShouldBeEqualAccordingToCompareToMethods(second, first);

            ShouldBeEqualAccordingToOperators(first, second);
            ShouldBeEqualAccordingToOperators(second, first);
        }

        [Test]
        public void greater_object_compares_greater_than_smaller_one()
        {
            var greater = new SomeComparableObject(2);
            var smaller = new SomeComparableObject(1);

            ShouldBeGreaterAccordingToCompareToMethods(greater, smaller);
            ShouldBeLesserThanAccordingToCompareToMethods(smaller, greater);

            ShouldBeGreaterThanAccordingToOperators(greater, smaller);
            ShouldBeLesserThanAccordingToOperators(smaller, greater);
        }

        [Test]
        public void any_object_compares_greater_than_null()
        {
            var some = new SomeComparableObject(1);

            ShouldBeGreaterAccordingToCompareToMethods(some, null);

            ShouldBeGreaterThanAccordingToOperators(some, null);
            ShouldBeLesserThanAccordingToOperators(null, some);
        }

        [Test]
        public void two_nulls_are_considered_equal()
        {
            ShouldBeEqualAccordingToOperators(null, null);
        }

        [Test]
        public void cannot_compare_objects_of_two_different_types()
        {
            var first = new SomeComparableObject(1);
            var second = "some string";

            Action comparing = () => first.CompareToObject(second);

            comparing.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void equality_comparing_objects_of_two_different_types_should_still_be_possible()
        {
            var first = new SomeComparableObject(1);
            var second = "some string";

            // ReSharper disable once SuspiciousTypeConversion.Global
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action equalityComparing = () => first.Equals(second);

            equalityComparing.ShouldNotThrow();
        }

        private void ShouldBeGreaterThanAccordingToOperators(
            SomeComparableObject first,
            SomeComparableObject second)
        {
            (first > second).Should().BeTrue();
            (first >= second).Should().BeTrue();
            (first == second).Should().BeFalse();
            (first <= second).Should().BeFalse();
            (first < second).Should().BeFalse();

            (first != second).Should().BeTrue();
        }

        private void ShouldBeLesserThanAccordingToOperators(SomeComparableObject first, SomeComparableObject second)
        {
            (first > second).Should().BeFalse();
            (first >= second).Should().BeFalse();
            (first == second).Should().BeFalse();
            (first <= second).Should().BeTrue();
            (first < second).Should().BeTrue();

            (first != second).Should().BeTrue();
        }

        private void ShouldBeEqualAccordingToOperators(
            SomeComparableObject first,
            SomeComparableObject second)
        {
            (first > second).Should().BeFalse();
            (first >= second).Should().BeTrue();
            (first == second).Should().BeTrue();
            (first <= second).Should().BeTrue();
            (first < second).Should().BeFalse();

            (first != second).Should().BeFalse();
        }

        private static void ShouldBeLesserThanAccordingToCompareToMethods(
            SomeComparableObject smaller,
            SomeComparableObject greater)
        {
            smaller.CompareTo(greater).Should().BeNegative();
            smaller.CompareToObject(greater).Should().BeNegative();
        }

        private static void ShouldBeGreaterAccordingToCompareToMethods(
            SomeComparableObject greater,
            SomeComparableObject smaller)
        {
            greater.CompareTo(smaller).Should().BePositive();
            greater.CompareToObject(smaller).Should().BePositive();
        }

        private static void ShouldBeEqualAccordingToCompareToMethods(
            SomeComparableObject first,
            SomeComparableObject second)
        {
            first.CompareTo(second).Should().Be(0);
            first.CompareToObject(second).Should().Be(0);
        }

        class SomeComparableObject : ComparableObject<SomeComparableObject>
        {
            private readonly int _value;

            public SomeComparableObject(int value) => _value = value;

            protected override int CompareToNotNull(SomeComparableObject notNullOther) =>
                _value.CompareTo(notNullOther._value);

            protected override int GenerateHashCode() => _value;
        }
    }

    public static class ComparableExtensions
    {
        public static int CompareToObject(this IComparable comparable, object other)
        {
            return comparable.CompareTo(other);
        }
    }
}
