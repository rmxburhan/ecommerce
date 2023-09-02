namespace Ecommerce.Api.models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}