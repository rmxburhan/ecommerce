namespace Ecommerce.Api.models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; } = string.Empty;
    public long Price { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}