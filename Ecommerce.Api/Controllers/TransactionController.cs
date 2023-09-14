using System.Data;
using System;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Ecommerce.Api.dto.transaction;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

/// <summary>
/// This endpoint is for admin only
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ApiDataContext dataContext;

    public TransactionController(ApiDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    /// <summary>
    /// get all orders from customer in admin panel
    /// Filter :
    /// 1. OrderStatus
    /// 2. Bydate
    /// 3. PaymentStatus
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var transactions = await dataContext.OrderHeaders.OrderByDescending(x => x.CreatedAt).Where(x => x.CancelledAt == null && x.UserId == user.Id).ToListAsync();

        return Ok(transactions);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatus request, Guid id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);
        if (user == null)
            return Unauthorized();

        var transaction = await dataContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id && x.CancelledAt == null);

        if (transaction == null)
            return NotFound();

        transaction.OrderStatus = request.OrderStatus;
        transaction.UpdatedAt = DateTime.UtcNow;
        dataContext.OrderHeaders.Update(transaction);

        await dataContext.SaveChangesAsync();
        return Ok(transaction);
    }
    [Authorize]
    [HttpPut("cancel/{id:guid}")]
    public async Task<IActionResult> AcceptCancelOrder(CancelOrderRequest reqeust, Guid id)
    {
        var transaction = await dataContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id && x.CancelledAt == null);

        if (transaction == null)
            return NotFound();

        transaction.CancelledAt = DateTime.UtcNow;
        transaction.CancelledMessage = reqeust.Message;
        transaction.UpdatedAt = DateTime.UtcNow;

        dataContext.OrderHeaders.Update(transaction);
        await dataContext.SaveChangesAsync();

        return Ok(transaction);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddTransaction(AddTransactionRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.DeletedAt == null);
        if (user == null)
            return Unauthorized();

        var orderHeader = new OrderHeader
        {
            OrderNumber = Guid.NewGuid().ToString(),
            Notes = "",
            UserId = user.Id,
            AddressId = request.AddressId,
            PaymentDeadline = DateTime.UtcNow.AddDays(1),
            ResiPengiriman = "",
            OrderStatus = OrderStatus.MenungguRespon,
            CreatedAt = DateTime.UtcNow,
        };
        dataContext.OrderHeaders.Add(orderHeader);
        var charts = new List<Chart>();
        foreach (var item in request.CartId)
        {
            var chart = await dataContext.Charts.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == item && x.DeletedAt == null);

            if (chart == null)
                continue;

            var orderDetail = new OrderDetail
            {
                Qty = chart.Qty,
                ProductId = chart.ProductId,
                Notes = chart.Notes,
                CreatedAt = DateTime.UtcNow,
                OrderHeaderId = orderHeader.Id
            };
            dataContext.OrderDetails.Add(orderDetail);
            charts.Add(chart);
        }
        orderHeader.TotalPrice = charts.Sum(x => x.Product.Price * x.Qty);
        dataContext.Charts.RemoveRange(charts);
        await dataContext.SaveChangesAsync();
        return Ok(orderHeader);
    }
}