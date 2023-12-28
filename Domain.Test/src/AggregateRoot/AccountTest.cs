using Domain.Tests;
using Domain.ValueObject;

namespace Domain.AggregateRoot.Tests;

public class AccountTest
{
    [Fact]
    public void CalculatesBalance()
    {
        AccountId accountId = new(1);
        var account = AccountTestData.DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow([
              ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(999))
              .Build(),
          ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(1))
              .Build()
            ]))
            .Build();
        var balance = account.CalculateBalance();
        Assert.Equal(Money.Of(1555), balance);
    }

    [Fact]
    public void WithdrawalSucceeds()
    {
        AccountId accountId = new(1);
        var account = AccountTestData.DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow([
              ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(999))
              .Build(),
          ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(1))
              .Build()
            ]))
            .Build();
        var success = account.Withdraw(Money.Of(555), new AccountId(99));
        Assert.True(success);
        Assert.Equal(3, account.ActivityWindow.GetActivities().Count);
        Assert.Equal(Money.Of(1000), account.CalculateBalance());
    }

    [Fact]
    public void WithdrawalFailure()
    {
        AccountId accountId = new(1);
        var account = AccountTestData.DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow([
              ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(999))
              .Build(),
          ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(1))
              .Build()
            ]))
            .Build();
        var success = account.Withdraw(Money.Of(1556), new AccountId(99));
        Assert.False(success);
        Assert.Equal(2, account.ActivityWindow.GetActivities().Count);
        Assert.Equal(Money.Of(1555), account.CalculateBalance());
    }

    [Fact]
    public void DepositSuccess()
    {
        AccountId accountId = new(1);
        var account = AccountTestData.DefaultAccount()
            .WithAccountId(accountId)
            .WithBaselineBalance(Money.Of(555))
            .WithActivityWindow(new ActivityWindow([
              ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(999))
              .Build(),
          ActivityTestData.DefaultActivity()
              .WithTargetAccount(accountId)
              .WithMoney(Money.Of(1))
              .Build()
            ]))
            .Build();
        var success = account.Deposit(Money.Of(445), new AccountId(99));
        Assert.True(success);
        Assert.Equal(3, account.ActivityWindow.GetActivities().Count);
        Assert.Equal(Money.Of(2000), account.CalculateBalance());
    }
}