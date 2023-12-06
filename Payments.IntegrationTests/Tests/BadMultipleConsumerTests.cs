using Payments.Contracts;

namespace IntegrationTests.Tests;

public sealed class BadMultipleConsumerTests : IClassFixture<PaymentsFixture>
{
    private readonly PaymentsFixture _fixture;

    public BadMultipleConsumerTests(PaymentsFixture fixture)
    {
        _fixture = fixture;
    }
    
    // Run multiple times
    [Fact]
    public async Task ItWillCreateSometing_WhenMessagePublished()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        await _fixture.BusTestHarness.Bus.Publish(new SomeMessageWithMultipleConsumers(id));
        
        // The problem: We are waiting for the message to be consumed. However, it's not consumed just once. Therefor
        // the test execution will continue but will be flaky because only one of the consumers will actually have finished
        // and we don't know which one.
        await _fixture.BusTestHarness.Consumed.Any<SomeMessageWithMultipleConsumers>(x => x.Context.Message.Id == id);
        
        // Assert
        var someThing = _fixture.DbContext.SomeThings.SingleOrDefault(x => x.Id == id);
        Assert.NotNull(someThing);
    }
}