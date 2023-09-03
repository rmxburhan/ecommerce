using System.Globalization;
namespace Ecommerce.Api.models;

public class Address
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    public string? Notes { get; set; } = string.Empty;
    public Double? Lat { get; set; }
    public Double? Lng { get; set; }
    public AddressType AddressType { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
