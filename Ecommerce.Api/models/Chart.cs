namespace Ecommerce.Api.models;

public class Chart
{
    public Guid Id { get; set; }
    public int Qty { get; set; } = 1;
    public string Notes { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}