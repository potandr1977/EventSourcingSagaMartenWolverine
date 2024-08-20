using Persons.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Persons.Write.CreatePersonUseCase;

/// <summary>
/// EndPoint добавления новой персоны.
/// </summary>
[WolverineHandler]
public static class CreatePersonEndPoint
{
    [WolverinePost("person/start-create-person-saga")]
    public static async Task<CreatePersonSagaResponse> Handle(CreatePersonCommand createPersonCommand, IMessageBus bus)
    {
        var sagaId = $"CreatePersonSaga-{Guid.NewGuid()}";
        // отправляем в wolverine событие начинающую сагу добавления новой персоны.
        // счёт будет добавлен, если в сагу отправить сообщение PersonApproved.
        await bus.PublishAsync(
            new CreatePersonSagaStarted(
                sagaId,
                createPersonCommand.Name,
                createPersonCommand.Inn));

        return new CreatePersonSagaResponse(sagaId);
    }
}
