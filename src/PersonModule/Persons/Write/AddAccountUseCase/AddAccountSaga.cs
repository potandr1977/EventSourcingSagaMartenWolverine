using Persons.Contracts;
using Wolverine;

namespace Persons.Write.AddAccountUseCase;

/// <summary>
/// Сага добавления аккаунта. 
/// </summary>
public class AddAccountSaga : Saga
{
    /// <summary>
    /// Идентификатор саги.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Идентификатор персоны.
    /// </summary>
    public string PersonId { get; set; }

    /// <summary>
    /// Наименование аккаунта.
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// Обработчик старта саги. 
    /// Название Start зарезервировано Wolverine. 
    /// Сообщение принимаемое в этом методе в качетсве первого параметра - будет считаться стартовым
    /// сообщением саги.
    /// Стартовый обработчик должен вернуть сагу. 
    /// В нашем случае сагу и сообщение завершающее сагу по таймауту.
    /// </summary>
    /// <param name="addAccountSagaStarted">Стартовое сообщение саги</param>
    /// <returns></returns>
    public static (AddAccountSaga, AddAccountTimeoutExpired) Start(AddAccountSagaStarted addAccountSagaStarted) => (new AddAccountSaga
    {
        //заполняем состояние саги данными.
        Id = addAccountSagaStarted.AddAccountSagaId,
        PersonId = addAccountSagaStarted.PersonId,
        AccountName = addAccountSagaStarted.AccountName
    },
    new AddAccountTimeoutExpired(addAccountSagaStarted.AddAccountSagaId));

    /// <summary>
    /// Успешное завершение саги, добавляем аккаунт.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="addAccountService">сервис добавления счёта.</param>
    public async void Handle(AccountApproved _, IAddAccountService addAccountService)
    {
        // Обращаемся к сервису добавления аккаунта,
        // отправляя туда данные из состояния саги.
        await addAccountService.CreateAccount(PersonId, AccountName);

        // Завершаем сагу.
        MarkCompleted();
    }

    /// <summary>
    /// Хэндлер отрицательного завершения саги.
    /// MarkCompleted - закрывает сагу.
    /// </summary>
    /// <param name="_"></param>
    public void Handle(AccountRejected _) => MarkCompleted();

    /// <summary>
    /// Хэндлер завершения саги по таймауту.
    /// MarkCompleted - закрывает сагу.
    /// </summary>
    /// <param name="_"></param>
    public void Handle(AddAccountTimeoutExpired _) => MarkCompleted();
}