using System;
using System.Collections.Generic;

namespace SimpleValueObjects.Tests
{
    public class PositiveInt : WrapperComparableObject<PositiveInt, int>
    {
        public PositiveInt(int value) : base(value)
        {
            if (value < 0)
            {
                throw new ArgumentException(
                    $"Cannot create an instance of {nameof(PositiveInt)} " +
                    $"with negative value: {value}.",
                    nameof(value));
            }
        }
    }

    public class ShortString : WrapperEquitableObject<ShortString, string>
    {
        public ShortString(string value) : base(value)
        {
            if (value.Length > 10)
            {
                throw new ArgumentException(
                    $"{nameof(ShortString)} cannot be longer than 10 characters, " +
                    $"but found '{value}' which is {value.Length} characters long.",
                    nameof(value));
            }
        }
    }

    internal class SpecificMonth : AutoComparableObject<SpecificMonth>
    {
        public int Year { get; }
        public int Month { get; }

        public SpecificMonth(int year, int month)
        {
            Year = year;
            Month = month;
        }

        protected override IEnumerable<IComparable> GetValuesForComparison()
        {
            yield return Year;
            yield return Month;
        }
    }
}
