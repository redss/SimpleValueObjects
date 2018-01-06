using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleValueObjects.Tests
{
    internal class WrapperComparableObjectTests
    {
        [Datapoints]
        public readonly int[] _intDataset =
        {
            int.MinValue,
            -123,
            0,
            123,
            int.MaxValue
        };

        [Theory]
        public void wrapper_objects_compare_like_their_wrapped_values(int firstValue, int secondValue)
        {
            var firstWrapper = new ValueWrapper(firstValue);
            var secondWrapper = new ValueWrapper(secondValue);

            var valueComparison = Comparer<int>.Default.Compare(firstValue, secondValue);
            var wrapperComparison = Comparer<ValueWrapper>.Default.Compare(firstWrapper, secondWrapper);

            valueComparison.Should().Be(wrapperComparison);
        }

        [Theory]
        public void wrappers_hash_code_is_wrapped_values_has_code(int value)
        {
            new ValueWrapper(value).GetHashCode().Should().Be(value.GetHashCode());
        }

        [Theory]
        public void wrapper_string_representation_is_same_as_wrapped_string_representation(int value)
        {
            var wrapper = new ValueWrapper(value);

            wrapper.ToString().Should().Be(value.ToString());
        }

        [Theory]
        public void any_wrapped_value_is_greater_than_wrapped_null(int value)
        {
            var firstWrapper = new ReferenceWrapper(new ValueWrapper(value));
            var secondWrapper = new ReferenceWrapper(null);

            Comparer<ReferenceWrapper>.Default.Compare(firstWrapper, secondWrapper)
                .Should().BePositive();
        }

        [Test]
        public void two_wrapped_nulls_are_equal()
        {
            var firstWrapped = new ReferenceWrapper(null);
            var secondWrapped = new ReferenceWrapper(null);

            Comparer<ReferenceWrapper>.Default.Compare(firstWrapped, secondWrapped)
                .Should().Be(0);
        }

        [Test]
        public void wrapped_nulls_hash_code_is_zero()
        {
            new ReferenceWrapper(null).GetHashCode().Should().Be(0);
        }

        [Test]
        public void string_representation_for_wrapped_null_is_some_default_value()
        {
            var wrapper = new ReferenceWrapper(null);

            wrapper.ToString().Should().Be("<null>");
        }

        class ReferenceWrapper : WrapperComparableObject<ReferenceWrapper, ValueWrapper>
        {
            public ReferenceWrapper(ValueWrapper value) : base(value)
            {
            }
        }

        class ValueWrapper : WrapperComparableObject<ValueWrapper, int>
        {
            public ValueWrapper(int value) : base(value)
            {
            }
        }
    }
}