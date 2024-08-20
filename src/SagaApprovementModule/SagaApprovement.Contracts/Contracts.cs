namespace SagaApprovement.Contracts;

public record ApproveAccountCommand(string SagaId);

public record ApprovePaymentCommand(string SagaId);

public record ApprovePersonCreationCommand(string SagaId);
