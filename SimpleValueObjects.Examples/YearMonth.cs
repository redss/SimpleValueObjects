using System;

namespace SimpleValueObjects.Examples
{
    public class YearMonth : ComparableObject<YearMonth>
    {
        public int Year { get; }
        public Month Month { get; }

        public YearMonth(int year, Month month)
        {
            Year = year;
            Month = month;

            if (!Enum.IsDefined(typeof(Month), month))
            {
                throw new ArgumentException($"Month {month} is not valid.");
            }
        }

        public YearMonth Next()
        {
            return Month == Month.December
                ? new YearMonth(Year + 1, Month.January)
                : new YearMonth(Year, Month + 1);
        }

        protected override int CompareToNotNull(YearMonth notNullOther)
        {
            return Year != notNullOther.Year
                ? Year - notNullOther.Year
                : Month - notNullOther.Month;
        }

        protected override int GenerateHashCode() => HashCodeCalculator.CalculateFromValues(Year, Month);

        public override string ToString() => $"{Month} {Year}";
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}