using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace Ecommerce.Api.models;
public class ApiDataContext : DbContext
{
    public ApiDataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
}