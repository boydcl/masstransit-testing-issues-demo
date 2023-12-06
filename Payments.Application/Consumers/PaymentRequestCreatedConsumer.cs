using MassTransit;
using Payments.Contracts;

namespace Application.Consumers;

public sealed class PaymentRequestCreatedConsumer : IConsumer<PaymentRequestCreated>
{
    public Task Consume(ConsumeContext<PaymentRequestCreated> context)
    {
        Console.WriteLine($"Sending payment link to customer {context.Message.CustomerEmail} for order {context.Message.OrderNumber} with amount {context.Message.Amount}");
        
        return Task.CompletedTask;
    }
}