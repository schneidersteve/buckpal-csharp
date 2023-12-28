using Domain.AggregateRoot;

namespace Application.Port;

public interface ILoadAccountPort
{
    Task<Account> LoadAccount(AccountId accountId, DateTime baselineDate);
}

public interface IAccountLock
{
    void LockAccount(AccountId accountId);
    void ReleaseAccount(AccountId accountId);
}

public interface IUpdateAccountStatePort
{
    Task UpdateActivities(Account account);
}