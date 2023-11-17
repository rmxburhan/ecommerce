using System.Diagnostics;
using System.Security.AccessControl;
using System;
using System.Security.Claims;
using Ecommerce.Api.dto.chart;
using Ecommerce.Api.dto.product;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// This api data is just specific for a user
/// so filter to the specific user by id found in token
/// </summary>
[ApiController]
[Route("api/charts")]
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
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var charts = await dataContext.Charts.Include(x => x.Product).OrderBy(x => x.Product.Name).Where(x => x.UserId == Guid.Parse(id) && x.DeletedAt == null).ToListAsync();
        return Ok(charts);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddChart(AddChartRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var chartExisted = await dataContext.Charts.FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.DeletedAt == null);

        if (chartExisted != null)
        {
            chartExisted.Qty += request.Qty;
            chartExisted.UpdatedAt = DateTime.UtcNow;
            dataContext.Charts.Update(chartExisted);
        }
        else
        {
            chartExisted = new Chart
            {
                Qty = request.Qty,
                ProductId = request.ProductId,
                Notes = request.Notes,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            dataContext.Charts.Add(chartExisted);
        }

        await dataContext.SaveChangesAsync();
        return Ok(chartExisted);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateChart(UpdateChartRequest request, Guid id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var chart = await dataContext.Charts.Include(x => x.Product).FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id && x.DeletedAt == null);
        if (chart == null)
            return NotFound();

        if (request.Qty.HasValue)
            chart.Qty = (int)request.Qty;
        if (!String.IsNullOrEmpty(request.Notes))
            chart.Notes = request.Notes;

        chart.UpdatedAt = DateTime.UtcNow;
        dataContext.Charts.Update(chart);
        await dataContext.SaveChangesAsync();
        return Ok(chart);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteChart(Guid id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var chart = await dataContext.Charts.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id && x.DeletedAt == null);
        if (chart == null)
            return NotFound();

        chart.DeletedAt = DateTime.UtcNow;
        dataContext.Charts.Update(chart);
        await dataContext.SaveChangesAsync();
        return NoContent();
    }
}