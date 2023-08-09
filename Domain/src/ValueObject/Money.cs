using System.Numerics;

namespace Domain.ValueObject;

public record Money(BigInteger Amount)
{
    public Boolean IsPositiveOrZero() => this.Amount.CompareTo(BigInteger.Zero) >= 0;

    public Boolean IsNegative() => this.Amount.CompareTo(BigInteger.Zero) < 0;

    public Boolean IsPositive() => this.Amount.CompareTo(BigInteger.Zero) > 0;

    public Boolean IsGreaterThanOrEqualTo(Money money) => this.Amount.CompareTo(money.Amount) >= 0;

    public Boolean IsGreaterThan(Money money) => this.Amount.CompareTo(money.Amount) >= 1;

    public static Money Of(long value) => new(new BigInteger(value));

    public static Money Add(Money a, Money b) => new(BigInteger.Add(a.Amount, b.Amount));

    // public Money Minus(Money money) => new Money(BigInteger.Subs(this.Amount.subtract(money.Amount));

    // public Money Plus(Money money) => new Money(this.Amount.add(money.Amount));

    // public static Money Subtract(Money a, Money b) => new Money(a.Amount.subtract(b.Amount));

    // public Money Negate() => new Money(this.Amount.negate());
}
