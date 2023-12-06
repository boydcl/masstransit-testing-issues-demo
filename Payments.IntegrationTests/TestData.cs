using Contracts;

namespace IntegrationTests;

public class TestData
{
    public static OrderConfirmed CreateOrderConfirmedMessage(string orderNumber)
    {
        var orderLines = new List<OrderLine>
        {
            new("SKU123", "Test Product", 1, 10.00m),
            new("SKU456", "Test Product 2", 2, 20.00m),
            new("SKU789", "Test Product 3", 3, 30.00m)
        };

        var customer = new Customer("John Doe", "john.doe@email.local");

        var orderConfirmed = new OrderConfirmed(orderNumber, orderLines.ToArray(), 60m, customer);
        return orderConfirmed;
    }
}