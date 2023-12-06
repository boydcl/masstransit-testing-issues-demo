using Contracts;

namespace IntegrationTests.Tests;

public sealed class BadCompetingPaymentTests(PaymentsFixture fixture) : IClassFixture<PaymentsFixture>
{
    // One of theses two tests will fail, because we are checking for an OrderConfirmed message to be consumed. 
    // However, the second test will say the message has already been consumed, because the first test already consumed
    // it. The IClassFixture is only instantiated once for the entire test class, therefor the harness that keeps
    // track of consumed messages is also only instantiated once.
    
    [Fact]
    public async Task ItWillCreateAPaymentRequest_WhenAnOrderIsConfirmed_Order1()
    {
        // Arrange
        var orderConfirmedMessage = TestData.CreateOrderConfirmedMessage("ACME-ORDER-A-1");

        // Act
        await fixture.BusTestHarness.Bus.Publish(orderConfirmedMessage);
        
        // The problem: We are checking for an OrderConfirmed message to be consumed. However, the other test will
        // already have put the message in the consumed list.
        await fixture.BusTestHarness.Consumed.Any<OrderConfirmed>();
        
        // Assert
        var paymentRequest = fixture.DbContext.PaymentRequests
            .SingleOrDefault(x => x.OrderNumber == orderConfirmedMessage.OrderNumber);
        
        Assert.NotNull(paymentRequest);
    }
    
    [Fact]
    public async Task ItWillCreateAPaymentRequest_WhenAnOrderIsConfirmed_Order2()
    {
        // Arrange
        var orderConfirmedMessage = TestData.CreateOrderConfirmedMessage("ACME-ORDER-A-2");

        // Act
        await fixture.BusTestHarness.Bus.Publish(orderConfirmedMessage);
        
        // The problem: We are checking for an OrderConfirmed message to be consumed. However, the other test will
        // already have put the message in the consumed list.
        await fixture.BusTestHarness.Consumed.Any<OrderConfirmed>();
        
        // Assert
        var paymentRequest = fixture.DbContext.PaymentRequests
            .SingleOrDefault(x => x.OrderNumber == orderConfirmedMessage.OrderNumber);
        
        Assert.NotNull(paymentRequest);
    }
}
