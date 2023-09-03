namespace Ecommerce.Api.dto.store;

public record UpdateStoreRequest(
    string? StoreName,
    string? Address,
    Double? Lat,
    Double? Lng
);