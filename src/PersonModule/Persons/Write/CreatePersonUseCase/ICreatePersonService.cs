using Domain.PersonAggregate;

namespace Persons.Write.CreatePersonUseCase;

/// <summary>
/// Сервис создания персоны.
/// </summary>
public interface ICreatePersonService
{
    /// <summary>
    /// Создаём персону.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="inn"></param>
    /// <returns></returns>
    Task<PersonAggregate> Create(string name, string inn);

    /// <summary>
    /// Получаем состояние персоны.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<PersonAggregate> Get(string personId);

    /// <summary>
    /// Изменяем инн.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="inn"></param>
    /// <returns></returns>
    Task<PersonAggregate> ChangeInn(string personId, string inn);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<PersonAggregate> ChangeName(string personId, string name);
}
