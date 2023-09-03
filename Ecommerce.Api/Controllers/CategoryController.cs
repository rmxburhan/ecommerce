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

    [HttpPost]
    public async Task<IActionResult> CreateCategory(AddCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.CategoryName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            DeletedAt = null
        };

        dataContext.Categories.Add(category);
        await dataContext.SaveChangesAsync();

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] FilterGetCategory filters)
    {
        var categories = await dataContext.Categories.Where(x => (string.IsNullOrEmpty(filters.Name) || x.Name.Contains(filters.Name))).ToListAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (category == null)
            return NotFound();

        return Ok(category);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest request, int id)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

        if (category == null)
            return NotFound();

        category.DeletedAt = DateTime.UtcNow;

        await dataContext.SaveChangesAsync();

        return NoContent();
    }
}