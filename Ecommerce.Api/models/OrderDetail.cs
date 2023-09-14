using System.Text.Json.Serialization;

namespace Ecommerce.Api.models;

public class OrderDetail
{
    public Guid Id { get; set; }
    public int Qty { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid OrderHeaderId { get; set; }
    [JsonIgnore]
    public virtual OrderHeader OrderHeader { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}