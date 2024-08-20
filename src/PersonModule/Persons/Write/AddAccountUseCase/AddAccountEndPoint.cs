using Persons.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Persons.Write.AddAccountUseCase;

/// <summary>
/// EndPoint добавления нового счёта персоны.
/// </summary>
[WolverineHandler]
public static class AddAccountEndPoints
{
    [WolverinePost("person/start-add-account-saga")]
    public static async Task<AddAccountSagaResponse> Handle(AddAccountCommand addAccountCommand, IMessageBus bus)
    {
        var sagaId = $"AddAccountSaga-{Guid.NewGuid()}";
        // отправляем в wolverine событие начинающую сагу добавления нового счёта.
        // счёт будет добавлен, если в сагу отправить сообщение AccountApproved.
        await bus.PublishAsync(
            new AddAccountSagaStarted(
                sagaId,
                addAccountCommand.PersonId,
                addAccountCommand.AccountName));

        return new AddAccountSagaResponse(sagaId);
    }
}
