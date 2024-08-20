using Domain.PersonAggregate;

namespace Persons.Write.AddAccountUseCase;

public interface IAddAccountService
{
    public Task<PersonAggregate> CreateAccount(string personId, string accountName);
}
