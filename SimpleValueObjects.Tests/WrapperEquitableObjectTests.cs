using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleValueObjects.Tests
{
    public class WrapperEquitableObjectTests
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

        [TestCase("some")]
        [TestCase("other")]
        public void can_compute_hash_code_for_a_wrapper(string value)
        {
            var wrapper = new SomeStringWrapper(value);

            wrapper.GetHashCode().Should().Be(value.GetHashCode());
        }

        [Test]
        public void hash_code_for_wrapped_null_is_zero()
        {
            var wrapper = new SomeStringWrapper(null);

            wrapper.GetHashCode().Should().Be(0);
        }

        [TestCase("some")]
        [TestCase("value")]
        [TestCase(null)]
        public void can_get_wrapped_value_from_wrapper(string value)
        {
            var wrapper = new SomeStringWrapper(value);

            wrapper.Value.Should().Be(value);
        }

        class SomeStringWrapper : WrapperEquitableObject<SomeStringWrapper, string>
        {
            public SomeStringWrapper(string value) : base(value)
            {
            }
        }
    }
}
