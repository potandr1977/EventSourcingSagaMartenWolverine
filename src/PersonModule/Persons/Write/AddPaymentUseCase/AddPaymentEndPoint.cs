using Persons.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Persons.Write.AddPaymentUseCase;

/// <summary>
/// Endpoint добавления платежа, возвращает идентификатор саги, который нужно отпарвить в один из 
/// ендпонитов из AppreoveEndPoints или RejectEndPoints.
/// </summary>
[WolverineHandler]
public static class AddPaymentEndPoint
{
    [WolverinePost("person/start-add-payment-saga")]
    public static async Task<AddPaymentSagaResponse> Handle(AddPaymentCommand addPaymentCommand, IMessageBus bus)
    {
        var sagaId = $"AddPaymentSaga-{Guid.NewGuid()}";
        // отправляем в wolverine событие начинающую сагу добавления нового платежа.
        // счёт будет добавлен, если в сагу отправить сообщение PaymentApproved.
        await bus.PublishAsync(
            new AddPaymentSagaStarted(
                sagaId,
                addPaymentCommand.PersonId,
                addPaymentCommand.AccountId,
                addPaymentCommand.Sum,
                addPaymentCommand.PaymentType));

        return new AddPaymentSagaResponse(sagaId);
    }
}
