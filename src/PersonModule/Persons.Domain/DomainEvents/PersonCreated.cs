namespace Domain.PersonAggregate.DomainEvents;

/// <summary>
/// Персона создана.
/// </summary>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="inn"></param>
public sealed class PersonCreated(string id, string name, string inn)
{
    public string Id { get; } = id;

    public string Name { get; } = name;

    public string Inn { get; } = inn;
}
