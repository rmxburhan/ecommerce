namespace Ecommerce.Api.models;

public class OrderDetail
{
    public int Id { get; set; }
    public int Qty { get; set; }
    public int ProductId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Product Product { get; set; }
    public int OrderHeaderId { get; set; }
    public virtual OrderHeader OrderHeader { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}