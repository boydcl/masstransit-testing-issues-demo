using Application.Data;
using Contracts;
using MassTransit;
using Payments.Contracts;

namespace Application.Consumers;

public sealed class OrderConfirmedConsumer(PaymentsDbContext _dbContext) : IConsumer<OrderConfirmed>
{
    public async Task Consume(ConsumeContext<OrderConfirmed> context)
    {
        var order = context.Message;

        var paymentRequest = new PaymentRequest
        {
            OrderNumber = order.OrderNumber,
            Amount = order.TotalPrice,
            CustomerEmail = order.Customer.Email
        };
        
        // Sleep for some time to simulate async work
        var delaySeconds = new Random().Next(1, 5);
        await Task.Delay(delaySeconds * 1000);

        await _dbContext.PaymentRequests.AddAsync(paymentRequest);
        await _dbContext.SaveChangesAsync();

        await context.Publish(
            new PaymentRequestCreated(
                paymentRequest.Id,
                paymentRequest.OrderNumber,
                paymentRequest.CustomerEmail,
                paymentRequest.Amount));
    }
}