namespace Ecommerce.Api.models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}