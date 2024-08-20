using Domain.PersonAggregate.DomainEvents;
using Domain.PersonAggregate.Enums;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Модель персоны, используется в проекции PersonWithSumProjection. В модель добавлено поле Saldo.
/// </summary>
public class PersonWithSum
{
    /// <summary>
    /// Идентификатор персоны.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// ФИО
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ИНН
    /// </summary>
    public string Inn { get; set; }

    /// <summary>
    /// Сальдо.
    /// </summary>
    public decimal Saldo { get; set; }

    /// <summary>
    /// Версия, обновляется с каждым событием.
    /// </summary>
    public long Version { get; private set; }

    /// <summary>
    /// Счета.
    /// </summary>
    public List<Account> Accounts = new List<Account>();

    /// <summary>
    /// Методы Apply будут вызваны проекцией при её построении.
    /// </summary>
    /// <param name="event"></param>
    public void Apply(PersonCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Inn = @event.Inn;
        Version++;
    }

    public void Apply(PersonNameChanged @event)
    {
        Name = @event.NewName;
        Version++;
    }

    public void Apply(PersonInnChanged @event)
    {
        Inn = @event.NewInn;
        Version++;
    }

    public void Apply(AccountCreated @event)
    {
        var account = new Account(@event.AccountId, @event.Name, new List<Payment>());
        Accounts.Add(account);
        Version++;
    }

    public void Apply(PaymentCreated @event)
    {
        var payment = new Payment(@event.Id, @event.Sum, @event.PaymentType);
        var account = Accounts.FirstOrDefault(x => x.Id == @event.AccountId) ?? throw new ArgumentNullException($"Счёт не найден с ид {@event.AccountId}");
        account.Payments.Add(payment);

        Saldo = @event.PaymentType == (int)PaymentTypeEnum.Credit ? Saldo + @event.Sum : Saldo - @event.Sum;

        Version++;
    }
}

/// <summary>
/// Платёж.
/// </summary>
public record Payment(string Id, decimal Sum, int PaymentType);

/// <summary>
/// Счёт.
/// </summary>
public record Account(string Id, string Name, List<Payment> Payments);
