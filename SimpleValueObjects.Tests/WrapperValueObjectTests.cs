using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleValueObjects.Tests
{
    public class WrapperValueObjectTests
    {
        [Datapoints]
        public readonly string[] _stringDatapoints = 
        {
            null,
            "some",
            "other"
        };

        [Theory]
        public void can_compare_two_wrappers(string firstValue, string secondValue)
        {
            var firstWrapped = new SomeStringWrapper(firstValue);
            var secondWrapped = new SomeStringWrapper(secondValue);

            var expectedResult = EqualityComparer<string>.Default.Equals(firstValue, secondValue);

            firstWrapped.Equals(secondWrapped).Should().Be(expectedResult);
        }

        [Theory]
        public void can_compute_hash_code_for_a_wrapper(string value)
        {
            Assume.That(value != null);

            var wrapper = new SomeStringWrapper(value);

            // ReSharper disable once PossibleNullReferenceException
            wrapper.GetHashCode().Should().Be(value.GetHashCode());
        }

        [Test]
        public void hash_code_for_wrapped_null_is_zero()
        {
            var wrapper = new SomeStringWrapper(null);

            wrapper.GetHashCode().Should().Be(0);
        }

        [Theory]
        public void can_get_wrapped_value_from_wrapper(string value)
        {
            var wrapper = new SomeStringWrapper(value);

            wrapper.Value.Should().Be(value);
        }

        [Theory]
        public void wrapper_string_representation_is_same_as_wrapped_string_representation(string value)
        {
            Assume.That(value != null);

            var wrapper = new SomeStringWrapper(value);

            wrapper.ToString().Should().Be(value);
        }

        [Test]
        public void string_representation_for_wrapped_null_is_some_default_value()
        {
            var wrapper = new SomeStringWrapper(null);

            wrapper.ToString().Should().Be("<null>");
        }

        class SomeStringWrapper : WrapperValueObject<SomeStringWrapper, string>
        {
            public SomeStringWrapper(string value) : base(value)
            {
            }
        }
    }
}
