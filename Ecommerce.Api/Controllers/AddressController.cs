using System.Net.Sockets;
using System;
using System.Security.Claims;
using Ecommerce.Api.dto.address;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using Swashbuckle.AspNetCore.Filters;


/// <summary>
/// This api data is just specific for a user
/// so filter to the specific user by id found in token
/// </summary>
namespace Ecommerce.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly ApiDataContext dataContext;

    public AddressController(ApiDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAddresses()
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var addreses = await dataContext.Addresses.Where(x => x.UserId == user.Id && x.DeletedAt == null).ToListAsync();

        return Ok(addreses);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAdress(Guid id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var addreses = await dataContext.Addresses.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id && x.DeletedAt == null);
        if (addreses == null)
            return NotFound();
        return Ok(addreses);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAddress(UpdateAddressRequest request, Guid id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var addreses = await dataContext.Addresses.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id && x.DeletedAt == null);
        if (addreses == null)
            return NotFound();

        if (!string.IsNullOrEmpty(request.Label))
            addreses.Label = request.Label;
        if (request.Lat.HasValue && request.Lng.HasValue)
            addreses.Lat = request.Lat; addreses.Lng = request.Lng;
        if (!string.IsNullOrEmpty(request.FullAddress))
            addreses.FullAddress = request.FullAddress;
        if (!string.IsNullOrEmpty(request.Notes))
            addreses.Notes = request.Notes;
        if (request.AddressType.HasValue)
            addreses.AddressType = (AddressType)request.AddressType;
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            addreses.PhoneNumber = request.PhoneNumber;

        addreses.UpdatedAt = DateTime.UtcNow;
        dataContext.Addresses.Update(addreses);
        await dataContext.SaveChangesAsync();

        return Ok(addreses);
    }


    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAddress(UpdateAddressRequest request, Guid id)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var addreses = await dataContext.Addresses.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Id == id && x.DeletedAt == null);
        if (addreses == null)
            return NotFound();

        addreses.DeletedAt = DateTime.UtcNow;
        dataContext.Addresses.Update(addreses);
        await dataContext.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAddress(AddAddressRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id_user = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id_user) && x.DeletedAt == null);

        if (user == null)
            return Unauthorized();

        var address = new Address
        {
            Label = request.Label,
            FullAddress = request.FullAddress,
            Notes = request.Notes,
            AddressType = request.AddressType,
            PhoneNumber = request.PhoneNumber,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        if (request.Lat.HasValue && request.Lng.HasValue)
        {
            address.Lat = request.Lat;
            address.Lng = request.Lng;
        }

        dataContext.Addresses.Add(address);
        await dataContext.SaveChangesAsync();

        return Ok(address);
    }
}