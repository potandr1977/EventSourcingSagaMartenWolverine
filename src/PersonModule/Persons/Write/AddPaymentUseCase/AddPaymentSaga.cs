using Domain.PersonAggregate.Enums;
using Persons.Contracts;
using Wolverine;

namespace Persons.Write.AddPaymentUseCase;

/// <summary>
/// Сага добавления платежа. 
/// </summary>
public class AddPaymentSaga : Saga
{
    /// <summary>
    /// Идентификатор саги.
    /// </summary>
    public string? Id { get; set; }

    public string PersonId { get; set; }

    public string AccountId { get; set; }

    public decimal Sum { get; set; }

    public PaymentTypeEnum PaymentType { get; set; }

    public static (AddPaymentSaga, AddPaymentTimeoutExpired) Start(AddPaymentSagaStarted addPaymentSagaStarted) => (new AddPaymentSaga
    {
        Id = addPaymentSagaStarted.AddPaymentSagaId,
        PersonId = addPaymentSagaStarted.PersonId,
        AccountId = addPaymentSagaStarted.AccountId,
        Sum = addPaymentSagaStarted.Sum,
        PaymentType = addPaymentSagaStarted.PaymentType,
    },
    new AddPaymentTimeoutExpired(addPaymentSagaStarted.AddPaymentSagaId));

    public void Handle(PaymentApproved _, IAddPaymentService addPaymentService)
    {
        // добавляем новый платёж
        addPaymentService.CreatePayment(PersonId, AccountId, Sum, PaymentType);
        // завершаем сагу.
        MarkCompleted();
    }

    /// <summary>
    /// Отрицательное завершение саги.
    /// </summary>
    /// <param name="_"></param>
    public void Handle(PaymentRejected _) => MarkCompleted();

    /// <summary>
    /// Завершение саги по таймауту.
    /// </summary>
    /// <param name="_"></param>
    public void Handle(AddPaymentTimeoutExpired _) => MarkCompleted();
}