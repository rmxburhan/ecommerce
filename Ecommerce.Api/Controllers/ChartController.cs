using System;
using System.Security.Claims;
using Ecommerce.Api.dto.chart;
using Ecommerce.Api.dto.product;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// This api data is just specific for a user
/// so filter to the specific user by id found in token
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChartController : ControllerBase
{
    private readonly ApiDataContext dataContext;

    public ChartController(ApiDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCharts()
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id));

        if (user == null)
            return Unauthorized();

        var charts = await dataContext.Charts.OrderBy(x => x.Product.Name).Where(x => x.UserId == int.Parse(id)).ToListAsync();
        return Ok(charts);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddChart(AddChartRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id));

        if (user == null)
            return Unauthorized();

        var chart = new Chart
        {
            Qty = request.Qty,
            ProductId = request.ProductId,
            Notes = request.Notes,
            UserId = int.Parse(id),
            CreatedAt = DateTime.UtcNow
        };
        dataContext.Charts.Add(chart);
        await dataContext.SaveChangesAsync();

        return Ok(chart);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChart(UpdateChartRequest request, int id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id_user));

        if (user == null)
            return Unauthorized();

        var chart = await dataContext.Charts.FirstOrDefaultAsync(x => x.UserId == int.Parse(id_user) && x.Id == id);
        if (chart == null)
            return NotFound();

        if (request.Qty.HasValue)
            chart.Qty = (int)request.Qty;
        if (!String.IsNullOrEmpty(request.Notes))
            chart.Notes = request.Notes;
        else
            chart.Notes = "";

        dataContext.Charts.Update(chart);
        await dataContext.SaveChangesAsync();

        return Ok(chart);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChart(int id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id_user));

        if (user == null)
            return Unauthorized();

        var chart = await dataContext.Charts.FirstOrDefaultAsync(x => x.UserId == int.Parse(id_user) && x.Id == id);
        if (chart == null)
            return NotFound();

        dataContext.Charts.Remove(chart);
        await dataContext.SaveChangesAsync();

        return NoContent();
    }
}