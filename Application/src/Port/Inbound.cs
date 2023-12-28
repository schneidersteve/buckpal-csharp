using Domain.AggregateRoot;
using Domain.ValueObject;

namespace Application.Port;

// ------------------------------
// Command-Query Separation (CQS)
// ------------------------------

interface ISendMoneyUseCase
{
    Task<bool> SendMoney(SendMoneyCommand command);
}

// TODO implement reflection free validating
public record SendMoneyCommand(AccountId SourceAccountId, AccountId TargetAccountId, Money Money) { }

interface IGetAccountBalanceQuery
{
    Task<Money> GetAccountBalance(AccountId accountId);
}