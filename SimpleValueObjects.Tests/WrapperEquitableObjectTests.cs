using FluentAssertions;
using NUnit.Framework;

namespace SimpleValueObjects.Tests
{
    public class WrapperEquitableObjectTests
    {
        [TestCase("same", "same", true)]
        [TestCase(null, null, true)]
        [TestCase("some", "other", false)]
        [TestCase(null, "some", false)]
        [TestCase("some", null, false)]
        public void can_compare_two_wrappers(string firstValue, string secondValue, bool expectedResult)
        {
            var wrapper = new SomeStringWrapper(firstValue);
            var other = new SomeStringWrapper(secondValue);

            wrapper.Equals(other).Should().Be(expectedResult);
        }

        [TestCase("some")]
        [TestCase("value")]
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
