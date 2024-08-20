namespace Domain.PersonAggregate.DomainEvents;
/// <summary>
/// Платёж создан.
/// </summary>
/// <param name="id"></param>
/// <param name="accountId"></param>
/// <param name="sum"></param>
/// <param name="paymentType"></param>
public sealed class PaymentCreated(string id, string accountId, decimal sum, int paymentType)
{
    public string Id { get; } = id;

    public string AccountId { get; } = accountId;

    public decimal Sum { get; } = sum;

    public int PaymentType { get; } = paymentType;
}
