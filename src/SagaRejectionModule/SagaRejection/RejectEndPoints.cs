using Persons.Contracts;
using SagaRejection.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Application.SagaRejectionEndPoints;

/// <summary>
/// Endpoint-ы отрицательного завершения саги добавления счёта, добавления платежа, создания персоны.
/// </summary>
[WolverineHandler]
public static class RejectEndPoints
{
    /// <summary>
    /// Отправляем в сагу добавления счёта сообщение запрещающее добавление счёта.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("reject-add-account-saga")]
    public static ValueTask Handle(RejectAccountCommand command, IMessageBus bus) => bus.PublishAsync(new AccountRejected(command.SagaId));

    
    /// <summary>
    /// Отправляем в сагу добавления платежа сообщение запрещающее добавление платежа.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("reject-add-payment-saga")]
    public static ValueTask Handle(RejectPaymentCommand command, IMessageBus bus) => bus.PublishAsync(new PaymentRejected(command.SagaId));
        
    /// <summary>
    /// Отправляем в сагу создания персоны сообщение запрещающее создание персоны.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("reject-person-creation-saga")]
    public static ValueTask Handle(RejectPersonCreationCommand command, IMessageBus bus) => bus.PublishAsync(new PersonRejected(command.SagaId));
}
