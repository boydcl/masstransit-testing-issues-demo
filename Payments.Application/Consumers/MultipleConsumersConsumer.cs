using Application.Data;
using MassTransit;
using Payments.Contracts;

namespace Application.Consumers;

public sealed class MultipleConsumersConsumer(PaymentsDbContext _dbContext) : IConsumer<SomeMessageWithMultipleConsumers>
{
    public async Task Consume(ConsumeContext<SomeMessageWithMultipleConsumers> context)
    {
        // Sleep for some time to simulate async work
        await Task.Delay(new Random().Next(50, 500));
        
        // Consumer one is writing something to the database.
        await _dbContext.SomeThings.AddAsync(new SomeThing { Id = context.Message.Id });
        await _dbContext.SaveChangesAsync();
    }
}

public sealed class MultipleConsumersConsumerTwo() : IConsumer<SomeMessageWithMultipleConsumers>
{
    public async Task Consume(ConsumeContext<SomeMessageWithMultipleConsumers> context)
    {
        // Sleep for some time to simulate async work
        await Task.Delay(new Random().Next(50, 500));
        
        // Consumer two is just logging something, but not writing to the database.
        Console.WriteLine("Consumer two is doing some work...");
    }
}