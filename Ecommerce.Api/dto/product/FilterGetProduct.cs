namespace Ecommerce.Api.dto.product;

public record FilterGetProduct(
    string? Name,
    int? CategoryId,
    int? FromPrice,
    int? ToPrice
);