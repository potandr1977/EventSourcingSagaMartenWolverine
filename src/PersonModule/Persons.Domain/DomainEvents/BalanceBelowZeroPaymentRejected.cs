namespace Persons.Domain.DomainEvents
{
    public sealed class BalanceBelowZeroPaymentRejected(string id, string accountId, decimal sum, int paymentType)
    {
        public string Id { get; } = id;

        public string AccountId { get; } = accountId;

        public decimal Sum { get; } = sum;

        public int PaymentType { get; } = paymentType;
    }
}
