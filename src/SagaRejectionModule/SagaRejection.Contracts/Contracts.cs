namespace SagaRejection.Contracts
{
    public record RejectAccountCommand(string SagaId);

    public record RejectPaymentCommand(string SagaId);

    public record RejectPersonCreationCommand(string SagaId);

}
