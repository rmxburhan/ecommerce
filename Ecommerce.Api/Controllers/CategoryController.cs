using System.Net;
using System.Security.AccessControl;
using Ecommerce.Api.dto.category;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ApiDataContext dataContext;

    public CategoryController(ApiDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(AddCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
        };

        dataContext.Categories.Add(category);
        await dataContext.SaveChangesAsync();

        return Ok(category);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] FilterGetCategory filters)
    {
        var categories = await dataContext.Categories.Where(x => (string.IsNullOrEmpty(filters.Name) || x.Name.Contains(filters.Name)) && x.DeletedAt == null).ToListAsync();

        return Ok(categories);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest request, Guid id)
    {
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (category == null)
            return NotFound();

        category.Name = request.Name;
        category.UpdatedAt = DateTime.UtcNow;

        dataContext.Categories.Update(category);

        await dataContext.SaveChangesAsync();

        return Ok(category);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (category == null)
            return NotFound();

        category.DeletedAt = DateTime.UtcNow;
        dataContext.Categories.Update(category);
        await dataContext.SaveChangesAsync();

        return NoContent();
    }
}