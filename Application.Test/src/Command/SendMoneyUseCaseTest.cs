using Application.Port;
using Domain.AggregateRoot;
using Domain.ValueObject;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;

namespace Application.Command.Tests;

#pragma warning disable CS0164

public class SendMoneyUseCaseImplTest
{
    [Fact]
    public async void TransactionSucceeds()
    {
        var loadAccountPort = Mock.Create<ILoadAccountPort>();
        var accountLock = Mock.Create<IAccountLock>();
        var updateAccountStatePort = Mock.Create<IUpdateAccountStatePort>();
        MoneyTransferProperties moneyTransferProperties = new();
        SendMoneyUseCaseImpl sendMoneyUseCase = new(
            loadAccountPort,
            accountLock,
            updateAccountStatePort,
            moneyTransferProperties);

    Given_a_source_account:
        var sourceAccount = Mock.Create<Account>();
        AccountId sourceAccountId = new(41);
        Mock.Arrange(() => sourceAccount.Id).Returns(sourceAccountId);
        Mock.Arrange(() =>
            loadAccountPort.LoadAccount(sourceAccountId, Arg.IsAny<DateTime>())
        ).ReturnsAsync(sourceAccount);
    And_a_target_account:
        var targetAccount = Mock.Create<Account>();
        AccountId targetAccountId = new(42);
        Mock.Arrange(() => targetAccount.Id).Returns(targetAccountId);
        Mock.Arrange(() =>
            loadAccountPort.LoadAccount(targetAccountId, Arg.IsAny<DateTime>())
        ).ReturnsAsync(targetAccount);
    And_money:
        var money = Money.Of(500);
    And_source_account_withdrawal_will_succeed:
        Mock.Arrange(() => sourceAccount.Withdraw(money, targetAccountId)).Returns(true);
    And_target_account_deposit_will_succeed:
        Mock.Arrange(() => targetAccount.Deposit(money, sourceAccountId)).Returns(true);

    When_money_is_send:
        SendMoneyCommand command = new(
                    sourceAccount.Id!,
                    targetAccount.Id!,
                    money);
        var success = await sendMoneyUseCase.SendMoney(command);

    Then_send_money_succeeds:
        Assert.True(success);

    And_source_account_is_locked:
        Mock.Assert(() => accountLock.LockAccount(sourceAccountId), Occurs.Once());
    And_source_account_is_released:
        Mock.Assert(() => accountLock.ReleaseAccount(sourceAccountId), Occurs.Once());

    And_target_account_is_locked:
        Mock.Assert(() => accountLock.LockAccount(targetAccountId), Occurs.Once());
    And_target_account_is_released:
        Mock.Assert(() => accountLock.ReleaseAccount(targetAccountId), Occurs.Once());

    And_accounts_have_been_updated:
        Mock.Assert(() => updateAccountStatePort.UpdateActivities(sourceAccount), Occurs.Once());
        Mock.Assert(() => updateAccountStatePort.UpdateActivities(targetAccount), Occurs.Once());
    }

    [Fact]
    public async void GivenWithdrawalFailsThenOnlySourceAccountIsLockedAndReleased()
    {
        var loadAccountPort = Mock.Create<ILoadAccountPort>();
        var accountLock = Mock.Create<IAccountLock>();
        var updateAccountStatePort = Mock.Create<IUpdateAccountStatePort>();
        MoneyTransferProperties moneyTransferProperties = new();
        SendMoneyUseCaseImpl sendMoneyUseCase = new(
            loadAccountPort,
            accountLock,
            updateAccountStatePort,
            moneyTransferProperties);

    Given_a_source_account:
        var sourceAccount = Mock.Create<Account>();
        AccountId sourceAccountId = new(41);
        Mock.Arrange(() => sourceAccount.Id).Returns(sourceAccountId);
        Mock.Arrange(() =>
            loadAccountPort.LoadAccount(sourceAccountId, Arg.IsAny<DateTime>())
        ).ReturnsAsync(sourceAccount);
    And_a_target_account:
        var targetAccount = Mock.Create<Account>();
        AccountId targetAccountId = new(42);
        Mock.Arrange(() => targetAccount.Id).Returns(targetAccountId);
        Mock.Arrange(() =>
            loadAccountPort.LoadAccount(targetAccountId, Arg.IsAny<DateTime>())
        ).ReturnsAsync(targetAccount);
    And_money:
        var money = Money.Of(500);
    And_source_account_withdrawal_will_fail:
        Mock.Arrange(() => sourceAccount.Withdraw(money, targetAccountId)).Returns(false);
    And_target_account_deposit_will_succeed:
        Mock.Arrange(() => targetAccount.Deposit(money, sourceAccountId)).Returns(true);

    When_money_is_send:
        SendMoneyCommand command = new(
                    sourceAccount.Id!,
                    targetAccount.Id!,
                    money);
        var success = await sendMoneyUseCase.SendMoney(command);

    Then_send_money_failed:
        Assert.False(success);

    And_source_account_is_locked:
        Mock.Assert(() => accountLock.LockAccount(sourceAccountId), Occurs.Once());
    And_source_account_is_released:
        Mock.Assert(() => accountLock.ReleaseAccount(sourceAccountId), Occurs.Once());

    And_target_account_is_not_locked:
        Mock.Assert(() => accountLock.LockAccount(targetAccountId), Occurs.Never());
    }
}