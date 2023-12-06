using Application.Data;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IntegrationTests;

public sealed class PaymentsFixture : WebApplicationFactory<Program>
{
    private readonly IServiceScope _scope;
    
    public PaymentsDbContext? DbContext { get; }
    public ITestHarness? BusTestHarness { get; }

    public PaymentsFixture()
    {
        _scope = Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
        BusTestHarness = _scope.ServiceProvider.GetRequiredService<ITestHarness>();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(
            configuration =>
            {
                configuration.Sources.Clear();
            });

        builder.ConfigureLogging(
            logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
        
        builder.ConfigureServices(
            services =>
            {
                // Configure the test harness and replace the original bus with the test harness in-memory bus.
                services.AddMassTransitTestHarness(
                    x =>
                    {
                        x.SetTestTimeouts(null, TimeSpan.FromSeconds(5));
                    });
            });

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scope.Dispose();
        }
        
        base.Dispose(disposing);
    }
}