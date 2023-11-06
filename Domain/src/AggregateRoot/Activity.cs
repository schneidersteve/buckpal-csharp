using Domain.ValueObject;

namespace Domain.AggregateRoot;

public record ActivityId(long Value);

public record Activity(
    ActivityId? Id,
    AccountId OwnerAccountId,
    AccountId SourceAccountId,
    AccountId TargetAccountId,
    DateTime Timestamp,
    Money Money
)
{
    public Activity(
        AccountId OwnerAccountId,
        AccountId SourceAccountId,
        AccountId TargetAccountId,
        DateTime Timestamp,
        Money Money
    ) : this(
        null,
        OwnerAccountId,
        SourceAccountId,
        TargetAccountId,
        Timestamp,
        Money
    )
    { }
}
