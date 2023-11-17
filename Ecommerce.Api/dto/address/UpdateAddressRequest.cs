using Ecommerce.Api.models;

namespace Ecommerce.Api.dto.address;

public record UpdateAddressRequest(
    string? Label,
    string? FullAddress,
    string? Notes,
    double? Lat,
    double? Lng,
    AddressType? AddressType,
    string? PhoneNumber);
