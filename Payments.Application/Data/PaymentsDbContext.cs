using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
    {
    }

    public required DbSet<PaymentRequest> PaymentRequests { get; set; }
    
    public required DbSet<SomeThing> SomeThings { get; set; }
}