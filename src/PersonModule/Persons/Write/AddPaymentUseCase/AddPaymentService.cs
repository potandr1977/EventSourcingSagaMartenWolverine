using Domain.PersonAggregate;
using Domain.PersonAggregate.Enums;
using Shared.Infrastructure;

namespace Persons.Write.AddPaymentUseCase;

/// <summary>
/// Сервис добавления платежа.
/// </summary>
/// <param name="personRepository"></param>
public class AddPaymentService(IRepository personRepository) : IAddPaymentService
{
    private readonly IRepository personRepository = personRepository;

    public async Task<PersonAggregate> CreatePayment(string personId, string accountId, decimal sum, PaymentTypeEnum paymentType)
    {
        var person = await personRepository.LoadAsync<PersonAggregate>(personId);
        var result = person.AddPayment(accountId, sum, paymentType);

        await personRepository.StoreAsync(person);

        if (result.DomainActionResultType == Domain.DomainActionResultTypeEnum.Failed)
        {
            throw new Exception(result.Description);
        }

        return person;
    }
}
