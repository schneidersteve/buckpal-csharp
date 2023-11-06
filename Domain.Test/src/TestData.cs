using Domain.AggregateRoot;
using Domain.ValueObject;

namespace Domain.Tests;

class AccountTestData
{

    public static AccountBuilder DefaultAccount()
    {
        return new AccountBuilder()
            .WithAccountId(new AccountId(42))
            .WithBaselineBalance(Money.Of(999))
            .WithActivityWindow(new ActivityWindow(
                ActivityTestData.DefaultActivity().Build(), ActivityTestData.DefaultActivity().Build()));
    }

    public class AccountBuilder
    {
        AccountId? accountId = null;
        Money? baselineBalance = null;
        ActivityWindow? activityWindow = null;

        public AccountBuilder WithAccountId(AccountId accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public AccountBuilder WithBaselineBalance(Money baselineBalance)
        {
            this.baselineBalance = baselineBalance;
            return this;
        }

        public AccountBuilder WithActivityWindow(ActivityWindow activityWindow)
        {
            this.activityWindow = activityWindow;
            return this;
        }

        public Account Build()
        {
            return Account.WithId(accountId!, baselineBalance!, activityWindow!);
        }
    }
}

class ActivityTestData
{
    public static ActivityBuilder DefaultActivity()
    {
        return new ActivityBuilder()
            .WithOwnerAccount(new AccountId(42))
            .WithSourceAccount(new AccountId(42))
            .WithTargetAccount(new AccountId(41))
            .WithTimestamp(DateTime.Now)
            .WithMoney(Money.Of(999));
    }

    public class ActivityBuilder
    {
        ActivityId? id = null;
        AccountId? ownerAccountId = null;
        AccountId? sourceAccountId = null;
        AccountId? targetAccountId = null;
        DateTime? timestamp = null;
        Money? money = null;

        ActivityBuilder WithId(ActivityId? id)
        {
            this.id = id;
            return this;
        }

        public ActivityBuilder WithOwnerAccount(AccountId accountId)
        {
            ownerAccountId = accountId;
            return this;
        }

        public ActivityBuilder WithSourceAccount(AccountId accountId)
        {
            sourceAccountId = accountId;
            return this;
        }

        public ActivityBuilder WithTargetAccount(AccountId accountId)
        {
            targetAccountId = accountId;
            return this;
        }

        public ActivityBuilder WithTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        public ActivityBuilder WithMoney(Money money)
        {
            this.money = money;
            return this;
        }

        public Activity Build()
        {
            return new Activity(id, ownerAccountId!, sourceAccountId!,
                targetAccountId!, (DateTime)timestamp!, money!);
        }
    }
}