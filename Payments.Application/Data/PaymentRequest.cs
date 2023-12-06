namespace Application.Data;

public class PaymentRequest
{
    public required string OrderNumber { get; set; }
    public required decimal Amount { get; set; }
    public required string CustomerEmail { get; set; }
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsPaid { get; set; }
}