using Domain.PersonAggregate.DomainEvents;
using Domain.PersonAggregate.Entities;
using Domain.PersonAggregate.Enums;
using Persons.Domain;
using Persons.Domain.DomainEvents;
using Shared.Abstractions;

namespace Domain.PersonAggregate;

/// <summary>
/// Агрегат персоны. 
/// </summary>
public sealed class PersonAggregate : Aggregate
{
    /// <summary>
    /// ФИО.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// ИНН.
    /// </summary>
    public string Inn { get; private set; }

    /// <summary>
    /// Счета. Эта коллекция может быть изменена только внутри класса и недоступна извне.
    /// </summary>
    private readonly List<Account> _accounts = new List<Account>();

    /// <summary>
    /// Коллекция данных о счетах которую мы выставляем наружу, извне изменить её не возможно.
    /// </summary>
    public IReadOnlyCollection<Account> Accounts { get => _accounts.AsReadOnly(); }

    /// <summary>
    /// Создаём персону.
    /// Создаём соответствующее событие и добавляем его список несохранённых событий.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="inn"></param>
    public PersonAggregate(string name, string inn)
    {
        var @event = new PersonCreated(
            $"Person-{Guid.NewGuid()}",
            name,
            inn);

        Apply(@event);

        AddUncommittedEvent(@event);
    }

    private PersonAggregate()
    {
    }

    /// <summary>
    /// Применяем событие PersonCreated, изменяем состояние персоны. 
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(PersonCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Inn = @event.Inn;
        Version++;
    }

    /// <summary>
    /// Применяем событие PersonNameChanged, изменяем состояние персоны. 
    /// Метод, впрочем как и все методы Apply, вызывается в двух случаях:
    /// 1. Из метода агрегата, который должен изменить состояние агрегата, 
    /// например метод SetName вызывает данный Apply, т.к. состояние мы можем менять только с помощью событий.
    /// 2. MartenDB вызывает методы Apply при чтении агрегата из базы, например методом
    /// session.Events.AggregateStreamAsync<T> в репозитории. По этой причине в этих методах нет проверок preconditions.
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(PersonNameChanged @event)
    {
        Name = @event.NewName;
        Version++;
    }

    protected void Apply(PersonInnChanged @event)
    {
        Inn = @event.NewInn;
        Version++;
    }

    protected void Apply(AccountCreated @event)
    {
        var account = Account.Create(@event.AccountId, @event.Name);
        _accounts.Add(account);
        Version++;
    }

    protected void Apply(AccountAlreadyExists @event)
    {
        Version++;
    }

    protected void Apply(PaymentCreated @event)
    {
        var payment = Payment.Create(@event.Id, @event.Sum, @event.PaymentType);

        var account = _accounts.FirstOrDefault(x => x.Id == @event.AccountId);
        account.AddPayment(payment);
        Version++;
    }

    protected void Apply(BalanceBelowZeroPaymentRejected @event)
    {
        Version++;
    }

    /// <summary>
    /// Измняем ФИО персоны (формируем событие и отправляем его на обработку)
    /// </summary>
    /// <param name="newName"></param>
    /// <exception cref="ArgumentException"></exception>
    public DomainActionResult SetName(string newName)
    {
        var @event = new PersonNameChanged(newName);

        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }

    /// <summary>
    /// Измняем ИНН персоны (формируем событие и отправляем его на обработку)
    /// </summary>
    /// <param name="newInn"></param>
    /// <exception cref="ArgumentException"></exception>
    public DomainActionResult SetInn(string newInn)
    {
        var @event = new PersonInnChanged(newInn);

        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }

    /// <summary>
    /// Добавляем новый счёт персоне.
    /// </summary>
    /// <param name="accountName"></param>
    /// <exception cref="ArgumentException"></exception>
    public DomainActionResult AddAccount(string accountName)
    {
        if (Accounts.Any(x => x.Name.Equals(accountName)))
        {
            var accountExistsEvent = new AccountAlreadyExists(
            $"{nameof(Account)}-{Guid.NewGuid()}",
            Name = accountName);

            Apply(accountExistsEvent);
            AddUncommittedEvent(accountExistsEvent);

            return new DomainActionResult(DomainActionResultTypeEnum.Failed, "Такой счёт уже существует");
        }

        var @event = new AccountCreated(
            $"{nameof(Account)}-{Guid.NewGuid()}",
            Name = accountName);

        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }

    /// <summary>
    /// Добавляем платёж на соответствующий счёт.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="sum"></param>
    /// <param name="paymentType"></param>
    public DomainActionResult AddPayment(string accountId, decimal sum, PaymentTypeEnum paymentType)
    {
        var account = _accounts.First(x => x.Id == accountId);

        if (paymentType == PaymentTypeEnum.Debit && GetAccountSaldo(account) - sum < 0)
        {
            //формируем событие отказа в приёме платежа из-за отрицательного баланса.
            var @rejectionEvent = new BalanceBelowZeroPaymentRejected(
            $"{nameof(Payment)}-{Guid.NewGuid()}",
            accountId,
            sum,
            (int)paymentType);

            // применяем событие.
            Apply(@rejectionEvent);
            // добавляем событие в список несохнанённых.
            AddUncommittedEvent(@rejectionEvent);

            return new DomainActionResult(DomainActionResultTypeEnum.Failed,"Баланс не может быть ниже нуля");
        }

        // формируем событие.
        var @event = new PaymentCreated(
            $"{nameof(Payment)}-{Guid.NewGuid()}",
            accountId,
            sum,
            (int)paymentType);
        // применяем событие.
        Apply(@event);
        // добавляем событие в список несохнанённых.
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }

    /// <summary>
    /// Получаем сальдо по счёту.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    private static decimal GetAccountSaldo(Account account)
    { 
        decimal saldo = 0;
        foreach(var payment in account.Payments) 
        {
            saldo = (payment.PaymentType == PaymentTypeEnum.Credit) ? saldo + payment.Sum : saldo - payment.Sum;
        }

        return saldo;
    }
}
