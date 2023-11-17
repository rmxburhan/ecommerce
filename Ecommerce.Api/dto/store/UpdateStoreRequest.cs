namespace Ecommerce.Api.dto.store;

public record UpdateStoreRequest(
    string? StoreName,
    string? Address,
    double? Lat,
    double? Lng);