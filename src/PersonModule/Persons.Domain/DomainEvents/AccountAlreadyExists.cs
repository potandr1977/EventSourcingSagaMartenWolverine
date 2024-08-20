namespace Domain.PersonAggregate.DomainEvents;

/// <summary>
/// Сообщение о добавлении счёта.
/// </summary>
/// <param name="accountId"></param>
/// <param name="name"></param>
public sealed class AccountAlreadyExists(string accountId, string name)
{
    public string AccountId { get; } = accountId;

    public string Name { get; } = name;
}
