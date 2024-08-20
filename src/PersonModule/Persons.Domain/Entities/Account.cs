using Shared.Abstractions;

namespace Domain.PersonAggregate.Entities;

/// <summary>
/// Сущность счёта, данные можно изменять только используя методы класса.
/// </summary>
public class Account : Entity
{
    /// <summary>
    /// платежи по счёту, доступны только внутри класса.
    /// </summary>
    private readonly List<Payment> _payments = new List<Payment>();

    protected Account()
    {
    }
    
    public string Name { get; private set; }

    /// <summary>
    /// данные о платежах, которые мы выставляем наружу, их можно только прочитать, но не изменить.
    /// </summary>
    public IReadOnlyCollection<Payment> Payments { get => _payments.AsReadOnly(); }

    internal protected static Account Create(string accountId, string name)
    {
        var account = new Account()
        {
            Id = accountId,
            Name = name,
        };

        return account;
    }

    internal protected void AddPayment(Payment payment) => _payments.Add(payment);
}
