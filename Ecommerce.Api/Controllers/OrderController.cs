using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Ecommerce.Api.dto.order;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ApiDataContext dataContext;

    public OrderController(ApiDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    /// <summary>
    /// This method is for het all order list for customer/user
    /// </summary>
    /// <returns></returns> <summary>
    /// HTTP 200; Success
    /// HTTP 400; Request body invalid;
    /// Http 401; Unauthorized
    /// </summary>
    /// <returns></returns>
    [Authorize, HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id_user));

        if (user == null)
            return Unauthorized();

        var orders = await dataContext.OrderHeaders.Include(x => x.OrderDetails).Where(x => x.UserId == user.Id).OrderByDescending(x => x.CreatedAt).ToListAsync();

        return Ok(orders);
    }

    /// <summary>
    /// AddOrder to place an order
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize, HttpPost]
    public async Task<IActionResult> AddOrder(AddOrderRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id_user));

        if (user == null)
            return NotFound();
        var orderHeader = new OrderHeader
        {
            OrderNumber = Guid.NewGuid().ToString(),
            UserId = user.Id,
            AddressId = request.AddressId,
            OrderStatus = OrderStatus.MenungguRespon,
        };
        dataContext.OrderHeaders.Add(orderHeader);
        List<Chart> charts = new List<Chart>();
        foreach (var item in request.ChartsId)
        {
            var chart = await dataContext.Charts.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == item);

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
            orderHeader.OrderDetails.Add(orderDetail);
        }
        orderHeader.TotalPrice = charts.Sum(x => x.Product.Price * x.Product.Price);
        orderHeader.PaymentDeadline = DateTime.UtcNow.AddDays(1);

        dataContext.OrderHeaders.Update(orderHeader);

        await dataContext.SaveChangesAsync();
        foreach (var item in charts)
        {
            dataContext.Charts.Remove(item);
        }
        await dataContext.SaveChangesAsync();
        return Ok();
    }


    /// <summary>
    /// Get single order data
    /// </summary>
    /// <param name="id">is for orderheader id</param>
    /// <returns></returns>
    [Authorize, HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id_user));
        if (user == null)
            return Unauthorized();

        var order = await dataContext.OrderHeaders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    /// <summary>
    /// Cancelling the order so uer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize, HttpPut("{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id_user));
        if (user == null)
            return Unauthorized();

        var orderHeader = await dataContext.OrderHeaders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);

        if (orderHeader == null)
            return NotFound();
        if (orderHeader.CancelledAt == null)
            return BadRequest();

        orderHeader.CancelledAt = DateTime.UtcNow;
        return Ok(orderHeader);
    }
}