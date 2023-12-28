using Application.Port;
using Domain.AggregateRoot;

namespace Application.Command;

//@Singleton
class NoOpAccountLock : IAccountLock
{
    public void LockAccount(AccountId accountId)
    {
        // do nothing
    }

    public void ReleaseAccount(AccountId accountId)
    {
        // do nothing
    }
}