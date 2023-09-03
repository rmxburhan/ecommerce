using Ecommerce.Api.models;

namespace Ecommerce.Api.dto.address;

public record AddAddressRequest
{
    public string Label { get; set; }
    public string FullAddress { get; set; }
    public string? Notes { get; set; }
    public Double? Lat { get; set; }
    public Double? Lng { get; set; }
    public AddressType AddressType { get; set; }
    public string PhoneNumber { get; set; }
}