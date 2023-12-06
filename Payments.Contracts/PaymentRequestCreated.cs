namespace Payments.Contracts;

public sealed record PaymentRequestCreated(Guid Id, string OrderNumber, string CustomerEmail, decimal Amount);