using Domain.PersonAggregate.Enums;
using JasperFx.Core;
using Wolverine;

namespace Persons.Contracts;

/// <summary>
/// Запрос получения персоны с информацией о сальдо.
/// </summary>
/// <param name="PersonId">Ид. персоны.</param>
public record GetPersonWithSumQuery(string PersonId);

/// <summary>
/// Запрос получения списка персон с сальдо.
/// </summary>
public record GetPersonsWithSumQuery();

/// <summary>
/// Команда добавления нового счёта персоны
/// </summary>
/// <param name="PersonId">Ид. персоны.</param>
/// <param name="AccountName">Наименование счёта.</param>
public record AddAccountCommand(string PersonId, string AccountName);

/// <summary>
/// Данный endpoint будет возвращать идентфикатор саги добавления нового счёта.
/// </summary>
/// <param name="SagaId">Ид. саги.</param>
public record AddAccountSagaResponse(string SagaId);

/// <summary>
/// Команда добавления платежа.
/// </summary>
/// <param name="PersonId"></param>
/// <param name="AccountId"></param>
/// <param name="Sum"></param>
/// <param name="PaymentType"></param>
public record AddPaymentCommand(string PersonId, string AccountId, decimal Sum, PaymentTypeEnum PaymentType);

/// <summary>
/// Респонс добавления платежа.
/// Возвращается 
/// </summary>
/// <param name="SagaId"></param>
public record AddPaymentSagaResponse(string SagaId);

/// <summary>
/// Команда создания персоны.
/// </summary>
/// <param name="Name"></param>
/// <param name="Inn"></param>
public record CreatePersonCommand(string Name, string Inn);

/// <summary>
/// Данный endpoint будет возвращать идентфикатор саги добавления новой персоны.
/// </summary>
/// <param name="SagaId"></param>
public record CreatePersonSagaResponse(string SagaId);

/// <summary>
/// Событие с которого стартует сага.
/// </summary>
public record CreatePersonSagaStarted(string CreatePersonSagaId, string Name, string Inn);

/// <summary>
/// Событие успешного завершения саги.
/// </summary>
public record PersonApproved(string Id);

/// <summary>
/// Событие отрицательного завершения саги.
/// </summary>

public record PersonRejected(string Id);


/// <summary>
/// Завершение саги по таймауту
/// </summary>
/// <param name="Id"></param>
public record PersonCreationTimeoutExpired(string Id) : TimeoutMessage(3.Minutes());

/// <summary>
/// Событие с которого стартует сага.
/// </summary>
/// <param name="AddPaymentSagaId"></param>
/// <param name="PersonId"></param>
/// <param name="AccountId"></param>
/// <param name="Sum"></param>
/// <param name="PaymentType"></param>
public record AddPaymentSagaStarted(string AddPaymentSagaId, string PersonId, string AccountId, decimal Sum, PaymentTypeEnum PaymentType);

/// <summary>
/// Событие успешного завершение саги.
/// </summary>
/// <param name="Id"></param>
public record PaymentApproved(string Id);

/// <summary>
/// Событие отрицательного завершения саги.
/// </summary>
/// <param name="Id"></param>
public record PaymentRejected(string Id);

/// <summary>
/// Событие истечения таймаугта добавления нового платежа.
/// </summary>
/// <param name="Id"></param>
public record AddPaymentTimeoutExpired(string Id) : TimeoutMessage(3.Minutes());

/// <summary>
/// Событие с которого стартует сага.
/// </summary>
/// <param name="AddAccountSagaId">Идентификатор саги, название зарезервировано.</param>
/// <param name="PersonId"></param>
/// <param name="AccountName"></param>
public record AddAccountSagaStarted(string AddAccountSagaId, string PersonId, string AccountName);

/// <summary>
/// Положительное завершение саги.
/// </summary>
/// <param name="Id"></param>
public record AccountApproved(string Id);

/// <summary>
/// Отрицательное завершение саги.
/// </summary>
/// <param name="Id"></param>
public record AccountRejected(string Id);

/// <summary>
/// Если сага не разрешилась положительно или отрицательно,
/// то она будет завершена по истечении 3 минут.
/// </summary>
/// <param name="Id"></param>
public record AddAccountTimeoutExpired(string Id) : TimeoutMessage(3.Minutes());