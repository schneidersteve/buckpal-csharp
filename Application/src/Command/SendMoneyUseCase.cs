using Application.Port;
using Domain.ValueObject;

namespace Application.Command;

//@Singleton
//@Transactional
public class SendMoneyUseCaseImpl(
    ILoadAccountPort loadAccountPort,
    IAccountLock accountLock,
    IUpdateAccountStatePort updateAccountStatePort,
    MoneyTransferProperties moneyTransferProperties
) : ISendMoneyUseCase
{
    public async Task<bool> SendMoney(SendMoneyCommand command)
    {
        CheckThreshold(command);

        var baselineDate = DateTime.Now.AddDays(-10);

        var sourceAccount = await loadAccountPort.LoadAccount(
            command.SourceAccountId,
            baselineDate
            );

        var targetAccount = await loadAccountPort.LoadAccount(
                command.TargetAccountId,
                baselineDate
                );

        var sourceAccountId = sourceAccount.Id ?? throw new ApplicationException("expected source account ID not to be empty");
        var targetAccountId = targetAccount.Id ?? throw new ApplicationException("expected target account ID not to be empty");

        accountLock.LockAccount(sourceAccountId);
        if (!sourceAccount.Withdraw(command.Money, targetAccountId))
        {
            accountLock.ReleaseAccount(sourceAccountId);
            return false;
        }

        accountLock.LockAccount(targetAccountId);
        if (!targetAccount.Deposit(command.Money, sourceAccountId))
        {
            accountLock.ReleaseAccount(sourceAccountId);
            accountLock.ReleaseAccount(targetAccountId);
            return false;
        }

        await updateAccountStatePort.UpdateActivities(sourceAccount);
        await updateAccountStatePort.UpdateActivities(targetAccount);

        accountLock.ReleaseAccount(sourceAccountId);
        accountLock.ReleaseAccount(targetAccountId);
        return true;
    }

    private void CheckThreshold(SendMoneyCommand command)
    {
        if (command.Money.IsGreaterThan(moneyTransferProperties.MaximumTransferThreshold))
        {
            throw new ThresholdExceededException(moneyTransferProperties.MaximumTransferThreshold, command.Money);
        }

    }
}

//@Singleton
public record MoneyTransferProperties
{
    public Money MaximumTransferThreshold { get; init; } = Money.Of(1_000_000);
}


class ThresholdExceededException(Money threshold, Money actual) : Exception
{
    public override string Message => $"Maximum threshold for transferring money exceeded: tried to transfer ${actual} but threshold is ${threshold}!";
}