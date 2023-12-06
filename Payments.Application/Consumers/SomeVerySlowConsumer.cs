using MassTransit;
using Payments.Contracts;

namespace Application.Consumers;

public sealed class SomeVerySlowConsumer : IConsumer<SomeMessageWithAVerySlowConsumer>
{
    public async Task Consume(ConsumeContext<SomeMessageWithAVerySlowConsumer> context)
    {
        await Task.Delay(15000);

        await context.Publish(new SomeMessageWithAVerySlowConsumerPublishedEvent());
    }
}