using Ecommerce.Api.Common;
using Ecommerce.Api.dto.product;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.IdentityModel.Tokens;

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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromForm] AddProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        if (!Directory.Exists(uploadPath.ProductImageUploadPath()))
            Directory.CreateDirectory(uploadPath.ProductImageUploadPath());

        string hashedFilename = Guid.NewGuid().ToString() + "_" + request.Image.FileName;
        string fileName = Path.Combine(uploadPath.ProductImageUploadPath(), hashedFilename);

        request.Image.CopyTo(new FileStream(fileName, FileMode.Create));

        product.Image = hashedFilename;

        dataContext.Products.Add(product);
        await dataContext.SaveChangesAsync();

        return Ok(product);
    }

    [Authorize, HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] FilterGetProduct filters)
    {
        var product = await dataContext.Products.OrderBy(x => x.Name).Where(x =>
        (string.IsNullOrEmpty(filters.Name) || x.Name.Contains(filters.Name)) &&
        (!filters.CategoryId.HasValue || x.CategoryId == filters.CategoryId) &&
        ((filters.FromPrice.HasValue ? x.Price >= filters.FromPrice : true) && (filters.ToPrice.HasValue ? x.Price <= filters.ToPrice : true)
         && x.DeletedAt == null)).ToListAsync();

        return Ok(product);
    }

    [Authorize, HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await dataContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [Authorize, HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequest request, Guid id)
    {
        var product = await dataContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (product == null)
            return NotFound();

        if (!string.IsNullOrEmpty(request.Name))
            product.Name = request.Name;

        if (!string.IsNullOrEmpty(request.Description))
            product.Name = request.Description;

        if (request.CategoryId.HasValue)
            product.CategoryId = (Guid)request.CategoryId;

        if (request.Image != null)
        {
            if (Directory.Exists(uploadPath.ProductImageUploadPath()))
                Directory.CreateDirectory(uploadPath.ProductImageUploadPath());

            string hashedFilename = Guid.NewGuid().ToString() + "_" + request.Image.FileName;
            string fileName = Path.Combine(uploadPath.ProductImageUploadPath(), hashedFilename);

            request.Image.CopyTo(new FileStream(Path.Combine(uploadPath.ProductImageUploadPath(), fileName), FileMode.Create));

            if (!product.Image.IsNullOrEmpty())
                if (System.IO.File.Exists(Path.Combine(uploadPath.ProductImageUploadPath(), product.Image)))
                    System.IO.File.Delete(Path.Combine(uploadPath.ProductImageUploadPath(), product.Image));

            product.Image = hashedFilename;
        }

        product.UpdatedAt = DateTime.UtcNow;

        dataContext.Products.Update(product);
        await dataContext.SaveChangesAsync();

        return Ok(product);
    }

    [Authorize, HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await dataContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (product == null)
            return NotFound();

        product.DeletedAt = DateTime.UtcNow;
        dataContext.Products.Update(product);
        await dataContext.SaveChangesAsync();
        return NoContent();
    }
}