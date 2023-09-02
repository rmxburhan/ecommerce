using Ecommerce.Api.Common;
using Ecommerce.Api.dto.product;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApiDataContext dataContext;
    private readonly IUploadPath uploadPath;

    public ProductController(ApiDataContext dataContext, IUploadPath uploadPath)
    {
        this.dataContext = dataContext;
        this.uploadPath = uploadPath;
    }

    [HttpGet]
    public async Task<IActionResult> AddProduct(AddProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        if (Directory.Exists(uploadPath.ImageUploadPath()))
            Directory.CreateDirectory(uploadPath.ImageUploadPath());

        string hashedFilename = Guid.NewGuid().ToString() + "_" + request.Image.FileName;
        string fileName = Path.Combine(uploadPath.ImageUploadPath(), hashedFilename);

        request.Image.CopyTo(new FileStream(fileName, FileMode.Create));

        product.Image = hashedFilename;

        dataContext.Products.Add(product);
        await dataContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] FilterGetProduct filters)
    {
        var product = await dataContext.Products.OrderBy(x => x.Name).Where(x =>
        (string.IsNullOrEmpty(filters.Name) || x.Name.Contains(filters.Name)) &&
        (!filters.CategoryId.HasValue || x.CategoryId == filters.CategoryId) &&
        ((filters.FromPrice.HasValue && filters.ToPrice.HasValue) ? (x.Price >= filters.FromPrice && x.Price <= filters.ToPrice) : true)
        ).ToListAsync();

        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await dataContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(UpdateProductRequest request, int id)
    {
        var product = await dataContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        if (!string.IsNullOrEmpty(request.Name))
            product.Name = request.Name;

        if (request.CategoryId.HasValue)
            product.CategoryId = (int)request.CategoryId;

        if (request.Image != null)
        {
            if (Directory.Exists(uploadPath.ImageUploadPath()))
                Directory.CreateDirectory(uploadPath.ImageUploadPath());

            string hashedFilename = Guid.NewGuid().ToString() + "_" + request.Image.FileName;
            string fileName = Path.Combine(uploadPath.ImageUploadPath(), hashedFilename);

            request.Image.CopyTo(new FileStream(fileName, FileMode.Create));

            if (System.IO.File.Exists(Path.Combine(uploadPath.ImageUploadPath(), product.Image)))
                System.IO.File.Delete(Path.Combine(uploadPath.ImageUploadPath(), product.Image));

            product.Image = hashedFilename;
        }

        dataContext.Products.Update(product);
        await dataContext.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await dataContext.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        dataContext.Products.Remove(product);
        await dataContext.SaveChangesAsync();
        return NoContent();
    }
}