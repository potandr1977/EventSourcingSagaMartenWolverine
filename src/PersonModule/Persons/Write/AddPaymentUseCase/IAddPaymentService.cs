using Domain.PersonAggregate;
using Domain.PersonAggregate.Enums;

namespace Persons.Write.AddPaymentUseCase;

public interface IAddPaymentService
{
    Task<PersonAggregate> CreatePayment(string personId, string accountId, decimal sum, PaymentTypeEnum paymentType);
}
