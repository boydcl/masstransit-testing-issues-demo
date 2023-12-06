using Application.Data;
using MassTransit;
using Payments.Contracts;

namespace Application.Consumers;

public sealed class SomeConsumerThatThrows(PaymentsDbContext _dbContext) : IConsumer<SomeMessageWithAFaultyConsumer>
{
    public async Task Consume(ConsumeContext<SomeMessageWithAFaultyConsumer> context)
    {
        throw new InvalidOperationException("some exception");
        
        await _dbContext.SomeThings.AddAsync(new SomeThing());
        await _dbContext.SaveChangesAsync();
    }
}