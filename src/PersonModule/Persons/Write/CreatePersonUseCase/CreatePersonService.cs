using Domain.PersonAggregate;
using Shared.Infrastructure;

namespace Persons.Write.CreatePersonUseCase;

/// <inheritdoc/>
public class CreatePersonService(IRepository personRepository) : ICreatePersonService
{
    private readonly IRepository personRepository = personRepository;

    /// <inheritdoc/>
    public async Task<PersonAggregate> Create(string name, string inn)
    {
        var person = new PersonAggregate(name, inn);

        await personRepository.StoreAsync(person);

        return person;
    }

    /// <inheritdoc/>
    public Task<PersonAggregate> Get(string personId) => personRepository.LoadAsync<PersonAggregate>(personId);

    /// <inheritdoc/>
    public async Task<PersonAggregate> ChangeInn(string personId, string inn)
    {
        var person = await personRepository.LoadAsync<PersonAggregate>(personId);
        var result = person.SetInn(inn);
        await personRepository.StoreAsync(person);

        if (result.DomainActionResultType == Domain.DomainActionResultTypeEnum.Failed)
        {
            throw new Exception(result.Description);
        }

        return person;
    }

    /// <inheritdoc/>
    public async Task<PersonAggregate> ChangeName(string personId, string name)
    {
        var person = await personRepository.LoadAsync<PersonAggregate>(personId);
        var result = person.SetName(name);

        await personRepository.StoreAsync(person);

        if (result.DomainActionResultType == Domain.DomainActionResultTypeEnum.Failed)
        {
            throw new Exception(result.Description);
        }

        return person;
    }
}
