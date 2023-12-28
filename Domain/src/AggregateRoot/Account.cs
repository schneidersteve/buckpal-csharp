using Domain.ValueObject;

namespace Domain.AggregateRoot;

public record AccountId(long Value);

public class Account
{
    public virtual AccountId? Id { get; }
    Money baselineBalance;
    public ActivityWindow ActivityWindow { get; }

    private Account(AccountId? id, Money baselineBalance, ActivityWindow activityWindow)
    {
        Id = id;
        this.baselineBalance = baselineBalance;
        this.ActivityWindow = activityWindow;
    }

    static Account WithoutId(Money baselineBalance, ActivityWindow activityWindow) => new(null, baselineBalance, activityWindow);
    public static Account WithId(AccountId id, Money baselineBalance, ActivityWindow activityWindow) => new(id, baselineBalance, activityWindow);

    public Money CalculateBalance() => baselineBalance + ActivityWindow.CalculateBalance(Id!);

    public virtual bool Withdraw(Money money, AccountId targetAccountId)
    {
        if (!MayWithdraw(money))
        {
            return false;
        }
        Activity withdrawal = new(Id!, Id!, targetAccountId, DateTime.Now, money);
        ActivityWindow.AddActivity(withdrawal);
        return true;
    }

    bool MayWithdraw(Money money) => (CalculateBalance() + money.Negate()).IsPositiveOrZero();

    public virtual bool Deposit(Money money, AccountId sourceAccountId)
    {
        Activity deposit = new(Id!, sourceAccountId, Id!, DateTime.Now, money);
        ActivityWindow.AddActivity(deposit);
        return true;
    }
}