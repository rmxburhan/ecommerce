namespace Ecommerce.Api.dto.product;

public class AddProductRequest
{
    public string Name { get; set; }
    public IFormFile Image { get; set; }
    public int Price { get; set; }
    public int CategoryId { get; set; }
}