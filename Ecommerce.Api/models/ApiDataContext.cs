using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Filters;

namespace Ecommerce.Api.models;
public class ApiDataContext : DbContext
{
    public ApiDataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
        .Entity<OrderHeader>()
        .Property(d => d.OrderStatus)
        .HasConversion(new EnumToStringConverter<OrderStatus>());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}