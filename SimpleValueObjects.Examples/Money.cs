using System;

namespace SimpleValueObjects.Examples
{
    public class Money : AutoEquitableObject<Money>
    {
        public Currency Currency { get; }
        public decimal Amount { get; }

        public Money(Currency currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;

            if (amount < 0)
            {
                throw new ArgumentException(
                    $"Money cannot have negative amount, but got {amount}.");
            }
        }

        public bool IsNothing => Amount == 0 || Currency == Currency.Blemflarcks;

        public override string ToString() => $"{Amount} {Currency}";
    }

    public enum Currency
    {
        USD,
        GBP,
        Blemflarcks
    }
}