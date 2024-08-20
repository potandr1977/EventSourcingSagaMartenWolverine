namespace Domain.PersonAggregate.DomainEvents;

/// <summary>
/// ФИО персоны изменены.
/// </summary>
/// <param name="newName"></param>
public sealed class PersonNameChanged(string newName)
{
    public string NewName { get; init; } = newName;
}
