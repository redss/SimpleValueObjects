using System;

namespace SimpleValueObjects.Examples
{
    public class IntRange : ValueObject<IntRange>
    {
        public int From { get; }
        public int To { get; }

        public IntRange(int from, int to)
        {
            From = from;
            To = to;

            if (From > To)
            {
                throw new InvalidOperationException(
                    $"From cannot be greater than To in IntRange, but got: {From}-{To}.");
            }
        }

        protected override bool EqualsNotNull(IntRange notNullOther)
        {
            return From == notNullOther.From && To == notNullOther.To;
        }

        protected override int GenerateHashCode()
        {
            return HashCodeCalculator.CalculateFromValues(From, To);
        }
    }
}