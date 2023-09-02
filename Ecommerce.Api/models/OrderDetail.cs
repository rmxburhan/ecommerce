namespace Ecommerce.Api.models;

public class OrderDetail
{
    public int Id { get; set; }
    public int Qty { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}