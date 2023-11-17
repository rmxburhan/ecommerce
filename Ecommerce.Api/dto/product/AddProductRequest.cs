namespace Ecommerce.Api.dto.product;

public record AddProductRequest(
    string Name,
    string Description,
    IFormFile Image,
    int Price,
    Guid CategoryId);
