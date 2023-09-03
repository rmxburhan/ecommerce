using Ecommerce.Api.models;

namespace Ecommerce.Api.dto.address;

public record UpdateAddressRequest
{
    public string? Label { get; set; } = string.Empty;
    public string? FullAddress { get; set; } = string.Empty;
    public string? Notes { get; set; } = string.Empty;
    public Double? Lat { get; set; }
    public Double? Lng { get; set; }
    public AddressType? AddressType { get; set; }
    public string? PhoneNumber { get; set; } = string.Empty;
}