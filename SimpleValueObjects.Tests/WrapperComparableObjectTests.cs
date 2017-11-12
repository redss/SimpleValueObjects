using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace SimpleValueObjects.Tests
{
    public class WrapperComparableObjectTests
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
        public void can_compare_wrapped_objects(int first, int second)
        {
            var firstWrapped = new SomeWrapped(first);
            var secondWrapped = new SomeWrapped(second);

            var valueComparison = Comparer<int>.Default.Compare(first, second);
            var wrappedComparison = Comparer<SomeWrapped>.Default.Compare(firstWrapped, secondWrapped);

            valueComparison.Should().Be(wrappedComparison);
        }

        // todo: test null cases

        class SomeWrapped : WrapperComparableObject<SomeWrapped, int>
        {
            public SomeWrapped(int value) : base(value)
            {
            }
        }
    }
}