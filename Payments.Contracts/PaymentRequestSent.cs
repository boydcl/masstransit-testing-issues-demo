namespace Payments.Contracts;

public sealed record PaymentRequestSent(Guid Id, string OrderNumber, string CustomerName, string CustomerEmail);