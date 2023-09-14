namespace Ecommerce.Api.models;

public class OrderHeader
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public long TotalPrice { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime PaymentDeadline { get; set; }
    public string ResiPengiriman { get; set; } = string.Empty;
    public OrderStatus OrderStatus { get; set; }
    public string? CancelledMessage { get; set; }
    public Guid AddressId { get; set; }
    public virtual Address Address { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public Guid? ReviewId { get; set; }
    public virtual Review? Review { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}
