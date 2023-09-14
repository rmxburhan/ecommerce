namespace Ecommerce.Api.dto.product;

public record FilterGetProduct(
    string? Name,
    Guid? CategoryId,
    int? FromPrice,
    int? ToPrice
);