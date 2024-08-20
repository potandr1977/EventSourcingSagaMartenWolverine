using Persons.Contracts;
using Wolverine;

namespace Persons.Write.CreatePersonUseCase;

/// <summary>
/// Сага создания персоны. 
/// после старта саги, ожидается сообщение о то что персона разрешена к сохранению, 
/// либо сообщение о том что она запрещена к сохранению.
/// Если ни одно из событий не случается в течении 3 минут, то сага завершается по таймауту в\
/// соответствующем обработчике.
/// </summary>
public class CreatePersonSaga : Saga
{
    /// <summary>
    /// Идентификатор саги.
    /// </summary>
    public string? Id { get; set; }

    public string Name { get; set; }

    public string Inn { get; set; }

    public static (CreatePersonSaga, PersonCreationTimeoutExpired) Start(CreatePersonSagaStarted createPersonSagaStarted) => (new CreatePersonSaga
    {
        Id = createPersonSagaStarted.CreatePersonSagaId,
        Name = createPersonSagaStarted.Name,
        Inn = createPersonSagaStarted.Inn,
    },
    new PersonCreationTimeoutExpired(createPersonSagaStarted.CreatePersonSagaId));

    /// <summary>
    /// Обработчик положительного завершения саги
    /// </summary>
    /// <param name="_"></param>
    /// <param name="createPersonService"></param>
    public async void Handle(PersonApproved _, ICreatePersonService createPersonService)
    {
        //сохраняем персону.
        await createPersonService.Create(Name, Inn);
        //завершаем сагу.
        MarkCompleted();
    }

    /// <summary>
    /// Обработчик отрицательного завершения саги.
    /// </summary>
    /// <param name="_"></param>
    public async void Handle(PersonRejected _) => MarkCompleted();

    /// <summary>
    /// Обработчик завершения саги по таймауту.
    /// </summary>
    /// <param name="_"></param>
    public void Handle(PersonCreationTimeoutExpired _) => MarkCompleted();
}