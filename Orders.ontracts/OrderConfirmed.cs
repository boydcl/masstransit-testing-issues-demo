namespace Contracts;

public record OrderConfirmed(string OrderNumber, OrderLine[] OrderLines, decimal TotalPrice, Customer Customer);

public record Customer(string Name, string Email);

public record OrderLine(string Sku, string Name, int Quantity, decimal Price);