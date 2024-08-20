using Domain.PersonAggregate;
using Shared.Infrastructure;

namespace Persons.Write.AddAccountUseCase;

/// <summary>
/// Сервис добавления счёта.
/// </summary>
/// <param name="personRepository"></param>
public class AddAccountService(IRepository personRepository) : IAddAccountService
{
    private readonly IRepository personRepository = personRepository;

    /// <summary>
    /// Создаём счёт.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="accountName"></param>
    /// <returns></returns>
    public async Task<PersonAggregate> CreateAccount(string personId, string accountName)
    {
        var person = await personRepository.LoadAsync<PersonAggregate>(personId);
        var result = person.AddAccount(accountName);

        await personRepository.StoreAsync(person);

        if (result.DomainActionResultType == Domain.DomainActionResultTypeEnum.Failed)
        { 
            throw new Exception(result.Description);
        }

        return person;
    }
}
