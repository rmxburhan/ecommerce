namespace Ecommerce.Api.dto.product;

public record UpdateProductRequest(
    string? Name,
    IFormFile? Image,
    string? Description,
    int? Price,
    Guid? CategoryId);
