using Marten;
using Newtonsoft.Json;
using Persons.Contracts;
using Wolverine.Http;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Endpoint получения данных о персонах.
/// </summary>
public static class GetPersonWithSumEndPoint
{
    /// <summary>
    /// Получаем персону по её идентификатору.
    /// В метод впрыскиваем сессию для получения read-модели.
    /// </summary>
    /// <param name="getPersonsWithSumCommand"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [WolverineGet("person/person")]
    public static async Task<string> Handle(GetPersonWithSumQuery getPersonsWithSumCommand, IQuerySession session)
    {
        var person = await session
            .Query<PersonWithSum>()
            .FirstOrDefaultAsync(c => c.Id == getPersonsWithSumCommand.PersonId) ?? throw new Exception($"Person not found.");

        return JsonConvert.SerializeObject(person, Formatting.Indented);
    }

    /// <summary>
    /// Получаем список всех персон (IRL это плохо, но для примера можно кмк.)
    /// Впрыскиваем в метод сессию для получения списка read-моделей.
    /// </summary>
    /// <param name="getPersonsWithSumCommand"></param>
    /// <param name="session"></param>
    /// <returns>Список персон с сальдо.</returns>
    /// <exception cref="Exception"></exception>
    [WolverineGet("person/persons")]
    public static async Task<string> Handle(GetPersonsWithSumQuery getPersonsWithSumCommand, IQuerySession session)
    {
        var persons = await session
            .Query<PersonWithSum>().ToListAsync() ?? throw new Exception($"Persons not found.");

        return JsonConvert.SerializeObject(persons, Formatting.Indented);
    }
}