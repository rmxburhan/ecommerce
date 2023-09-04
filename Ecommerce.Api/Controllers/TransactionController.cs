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
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await dataContext.OrderHeaders.OrderByDescending(x => x.CreatedAt).Where(x => x.CancelledAt == null).ToListAsync();

        return Ok(transactions);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatus request, int id)
    {
        var transaction = await dataContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
            return NotFound();

        transaction.OrderStatus = request.OrderStatus;
        dataContext.OrderHeaders.Update(transaction);

        await dataContext.SaveChangesAsync();
        return Ok(transaction);
    }

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> AcceptCancelOrder(CancelOrderRequest reqeust, int id)
    {
        var transaction = await dataContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
            return NotFound();

        transaction.CancelledAt = DateTime.UtcNow;
        transaction.CancelledMessage = reqeust.Message;

        dataContext.OrderHeaders.Update(transaction);
        await dataContext.SaveChangesAsync();

        return Ok(transaction);
    }
}