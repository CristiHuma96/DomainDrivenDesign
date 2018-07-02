using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Logic.Customers
{
    public class Money : ValueObject
    {
        private const decimal MaxMoneyAmount = 1_000_000;

        public decimal Value { get; }

        public bool IsZero => Value == 0;

        private Money(decimal value)
        {
            Value = value;
        }

        public static Result<Money> Create(decimal moneyAmount)
        {
            if (moneyAmount < 0)
                return Result.Fail<Money>("Money amount cannot be negative");

            if (moneyAmount > MaxMoneyAmount)
                return Result.Fail<Money>("Money amount cannot be greater than " + MaxMoneyAmount);

            if (moneyAmount % 0.01m > 0)
                return Result.Fail<Money>("Money amount cannot contain part of a penny");

            return Result.Ok(new Money(moneyAmount));
        }

        public static Money Of(decimal dollarAmount)
        {
            return Create(dollarAmount).Value;
        }

        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Value * multiplier);
        }

        public static Money operator +(Money money1, Money money2)
        {
            return new Money(money1.Value + money2.Value);
        }

        public static implicit operator decimal(Money money)
        {
            return money.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
