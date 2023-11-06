using System.Numerics;

namespace Domain.ValueObject.Tests;

public class MoneyTest
{
    [Fact]
    public void Of()
    {
        var money = Money.Of(42);
        Assert.Equal(new BigInteger(42), money.Amount);
    }

    [Fact]
    public void AddPlus()
    {
        var a = Money.Of(1);
        var b = Money.Of(2);
        var actual = a + b;
        var expected = a.Plus(b);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SubtractMinus()
    {
        var a = Money.Of(7);
        var b = Money.Of(3);
        var actual = a - b;
        var expected = a.Minus(b);
        Assert.Equal(expected, actual);
    }
}