using Domain.PersonAggregate.DomainEvents;
using Marten.Events.Aggregation;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Проекция событий агрегата PersonAggreate. Проекция вычисляет сальдо по каждому из агрегатов.
/// </summary>
public class PersonWithSumProjection : SingleStreamProjection<PersonWithSum>
{
    public PersonWithSumProjection()
    {
        // Вызываются методы Apply модели PersonWithSum
        ProjectEvent<PersonCreated>((item, @event) => item.Apply(@event));
        ProjectEvent<PersonInnChanged>((item, @event) => item.Apply(@event));
        ProjectEvent<PersonNameChanged>((item, @event) => item.Apply(@event));
        ProjectEvent<AccountCreated>((item, @event) => item.Apply(@event));
        // В этом Apply вычисляется сальдо.
        ProjectEvent<PaymentCreated>((item, @event) => item.Apply(@event));
    }
}