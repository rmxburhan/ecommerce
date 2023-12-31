using System.Globalization;
namespace Ecommerce.Api.models;

public class Address
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    public string? Notes { get; set; } = string.Empty;
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public AddressType AddressType { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
