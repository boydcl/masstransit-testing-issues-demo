using Application.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x => 
{
    // Using in memory bus as this is a demo project. Normally you would use an actual transport like
    // RabbitMQ/Azure Service Bus/Amazon SQS/...
    x.UsingInMemory();
    
    // Add all consumers in this assembly
    x.AddConsumers(typeof(Program).Assembly);
});

builder.Services.AddDbContext<PaymentsDbContext>(
    x =>
    {
        x.UseInMemoryDatabase("PaymentsDb");
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public partial class Program;