namespace Ecommerce.Api.models;

public class OrderHeader
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public int TotalPrice { get; set; }
    public string Notes { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int AddressId { get; set; }
    public string ResiPengiriman { get; set; }
    public virtual Address Address { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
