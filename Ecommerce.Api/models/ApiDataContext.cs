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
        modelBuilder.Entity<Address>()
        .Property(d => d.AddressType)
        .HasConversion(new EnumToStringConverter<AddressType>());

        modelBuilder
        .Entity<OrderHeader>()
        .Property(d => d.OrderStatus)
        .HasConversion(new EnumToStringConverter<OrderStatus>());

        modelBuilder.Entity<Store>()
        .HasIndex(db => db.Id)
        .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Chart> Charts { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

}