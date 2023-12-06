using Contracts;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Tests;

public class BadFlakyPaymentsTests(PaymentsFixture _fixture) : IClassFixture<PaymentsFixture>
{
    /// These test implementations would normally cause flaky tests. However, for demonstration purposes we intentionally
    /// made the consumer slow by adding a 1+ second sleep. This means that the consumer will always be slower than the
    /// assertions. This is not a realistic scenario, but it does demonstrate the problem.
    /// 
    /// In a real world scenario, these tests would be flaky (only fail sometimes), because the race outcome is random.
    /// Sometimes the consumer finishes on time before the assertion is made, sometimes it doesn't.
    
    /// <summary>
    /// This test will fail because masstransit is consuming the message async. By the time the assertion is made,
    /// the message has not been fully consumed yet.
    /// </summary>
    [Fact]
    public async Task ItWillCreateAPaymentRequest_WhenOrderConfirmed()
    {
        // Arrange
        var orderConfirmedMessage = TestData.CreateOrderConfirmedMessage("ACME-ORDER-B-1");

        // Act
        await _fixture.BusTestHarness.Bus.Publish(orderConfirmedMessage);
        
        // The problem: We are not waiting for the message to be consumed. Therefor the assertion will happen before
        // the consumer has finished.
        
        // Assert
        var paymentRequest = _fixture.DbContext.PaymentRequests
            .SingleOrDefault(x => x.OrderNumber == orderConfirmedMessage.OrderNumber);
        
        Assert.NotNull(paymentRequest);
    }

    /// <summary>
    /// This test will fail because even though we are awaiting the consumer to be finished. We're not awaiting the
    /// specific messages to be consumed. This means that the assertion is made before all expected messages are consumed.
    /// </summary>
    [Fact]
    public async Task ItWillCreateTwoPaymentsRequest_WhenTwoOrdersConfirmed()
    {
        // Arrange
        var orderConfirmedMessages = new[]
        {
            TestData.CreateOrderConfirmedMessage("ACME-ORDER-B-2"), TestData.CreateOrderConfirmedMessage("ACME-ORDER-B-3")
        };
        
        // Act
        await _fixture.BusTestHarness.Bus.Publish(orderConfirmedMessages[0]);
        await _fixture.BusTestHarness.Bus.Publish(orderConfirmedMessages[1]);
        
        // The problem: We are waiting for the OrderConfirmed message to be consumed. However, we are not waiting for
        // both SPECIFIC messages to be consumed. This await below only waits for either order 1, 2 or 3 to be consumed.
        await _fixture.BusTestHarness.Consumed.Any<OrderConfirmed>();
        
        // Assert
        var paymentRequests = await _fixture.DbContext.PaymentRequests
            .Where(x => orderConfirmedMessages.Select(o => o.OrderNumber).Contains(x.OrderNumber))
            .ToListAsync();
        
        Assert.Equal(2, paymentRequests.Count);
    }
}