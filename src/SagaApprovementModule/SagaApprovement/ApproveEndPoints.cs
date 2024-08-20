using Persons.Contracts;
using SagaApprovement.Contracts;
using Wolverine;
using Wolverine.Http;

namespace Application.SagaApprovmentEndPoints;

/// <summary>
/// EndPoint завершения саги добавления счёта, добавления платежа, создания персоны.
/// Во всех трёх обработчиках один из параметров - впрыск ссылки на объект шины сообщений.
/// </summary>
public static class ApproveEndPoints
{
    /// <summary>
    /// Отправляем в сагу добавления счёта сообщение разешающее добавление счёта.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("approve-add-account-saga")]
    public static ValueTask Handle(ApproveAccountCommand command, IMessageBus bus) => bus.PublishAsync(new AccountApproved(command.SagaId));

    /// <summary>
    /// Отправляем в сагу добавления платежа сообщение разешающее добавление платежа.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("approve-add-payment-saga")]
    public static ValueTask Handle(ApprovePaymentCommand command, IMessageBus bus) => bus.PublishAsync(new PaymentApproved(command.SagaId));
    

    /// <summary>
    /// Отправляем в сагу создания персоны сообщение разешающее создание персоны.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("approve-person-creation-saga")]
    public static ValueTask Handle(ApprovePersonCreationCommand command, IMessageBus bus) => bus.PublishAsync(new PersonApproved(command.SagaId));
}
