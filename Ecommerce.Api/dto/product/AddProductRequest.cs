namespace Ecommerce.Api.dto.product;

public class AddProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
    public int Price { get; set; }
    public Guid CategoryId { get; set; }
}