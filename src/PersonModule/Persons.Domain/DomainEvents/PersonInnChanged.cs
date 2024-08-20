namespace Domain.PersonAggregate.DomainEvents;

/// <summary>
/// ИНН изменён.
/// </summary>
/// <param name="newInn"></param>
public sealed class PersonInnChanged(string newInn)
{
    public string NewInn { get; } = newInn;
}
