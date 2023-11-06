using System.Numerics;
using Domain.AggregateRoot;
using Domain.Tests;

namespace Domain.ValueObject.Tests;

public class ActivityWindowTest
{
    [Fact]
    public void CalculatesStartTimestamp()
    {
        ActivityWindow window = new([
            ActivityTestData.DefaultActivity().WithTimestamp(StartDate()).Build(),
            ActivityTestData.DefaultActivity().WithTimestamp(InBetweenDate()).Build(),
            ActivityTestData.DefaultActivity().WithTimestamp(EndDate()).Build()
        ]);
        Assert.Equal(StartDate(), window.GetStartTimestamp());
    }

    [Fact]
    public void CalculatesEndTimestamp()
    {
        ActivityWindow window = new([
            ActivityTestData.DefaultActivity().WithTimestamp(StartDate()).Build(),
            ActivityTestData.DefaultActivity().WithTimestamp(InBetweenDate()).Build(),
            ActivityTestData.DefaultActivity().WithTimestamp(EndDate()).Build()
        ]);
        Assert.Equal(EndDate(), window.GetEndTimestamp());
    }


    [Fact]
    public void CalculatesBalance()
    {
        AccountId account1 = new(1);
        AccountId account2 = new(2);
        ActivityWindow window = new([
            ActivityTestData.DefaultActivity()
            .WithSourceAccount(account1)
            .WithTargetAccount(account2)
            .WithMoney(Money.Of(999))
            .Build(),
            ActivityTestData.DefaultActivity()
            .WithSourceAccount(account1)
            .WithTargetAccount(account2)
            .WithMoney(Money.Of(1))
            .Build(),
            ActivityTestData.DefaultActivity()
            .WithSourceAccount(account2)
            .WithTargetAccount(account1)
            .WithMoney(Money.Of(500))
            .Build()
        ]);
        Assert.Equal(Money.Of(-500), window.CalculateBalance(account1));
        Assert.Equal(Money.Of(500), window.CalculateBalance(account2));
    }

    static DateTime StartDate() => new(2019, 8, 3, 0, 0, 0);

    static DateTime InBetweenDate() => new(2019, 8, 4, 0, 0, 0);

    static DateTime EndDate() => new(2019, 8, 5, 0, 0, 0);
}