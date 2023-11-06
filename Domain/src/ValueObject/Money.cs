using System.Numerics;

namespace Domain.ValueObject;

public record Money(BigInteger Amount)
{
    public static Money ZERO = Money.Of(0);

    public Boolean IsPositiveOrZero() => this.Amount.CompareTo(BigInteger.Zero) >= 0;

    public Boolean IsNegative() => this.Amount.CompareTo(BigInteger.Zero) < 0;

    public Boolean IsPositive() => this.Amount.CompareTo(BigInteger.Zero) > 0;

    public Boolean IsGreaterThanOrEqualTo(Money money) => this.Amount.CompareTo(money.Amount) >= 0;

    public Boolean IsGreaterThan(Money money) => this.Amount.CompareTo(money.Amount) >= 1;

    public Money Plus(Money money) => new(BigInteger.Add(this.Amount, money.Amount));

    public Money Minus(Money money) => new(BigInteger.Subtract(this.Amount, money.Amount));

    public Money Negate() => new(BigInteger.Negate(this.Amount));

    public static Money Of(long value) => new(new BigInteger(value));

    public static Money operator +(Money a, Money b) => new(BigInteger.Add(a.Amount, b.Amount));

    public static Money operator -(Money a, Money b) => new(BigInteger.Subtract(a.Amount, b.Amount));
}
