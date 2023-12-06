using MassTransit;
using Payments.Contracts;

namespace IntegrationTests.Tests;

public sealed class BadFaultingPaymentsTests(PaymentsFixture _fixture) : IClassFixture<PaymentsFixture>
{
    [Fact]
    public async Task ItWillCreateSomeThing_WhenMessagePublished()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _fixture.BusTestHarness.Bus.Publish(new SomeMessageWithAFaultyConsumer(id));
        
        // The problem: We are waiting for the consumer to be consumed. And it did, but it failed, however our
        // test process doesn't know about that because the error is happening in the background. Tip; run it in debug
        // mode and catch exceptions or check the console log.
        await _fixture.BusTestHarness.Consumed.Any<SomeMessageWithAFaultyConsumer>(x => x.Context.Message.Id == id);
        
        // Assert
        var someThing = _fixture.DbContext.SomeThings.SingleOrDefault(x => x.Id == id);
        Assert.NotNull(someThing);
    }
}