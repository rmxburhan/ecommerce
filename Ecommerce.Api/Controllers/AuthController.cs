using System.ComponentModel.Design.Serialization;
using Ecommerce.Api.Common;
using Ecommerce.Api.dto.authentication;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator tokenGenerator;
    private readonly ApiDataContext dataContext;
    private readonly IPasswordHasher passwordHasher;

    public AuthController(IJwtTokenGenerator tokenGenerator, ApiDataContext dataContext, IPasswordHasher passwordHasher)
    {
        this.tokenGenerator = tokenGenerator;
        this.dataContext = dataContext;
        this.passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email && x.DeletedAt == null);

        if (user == null)
            return NotFound();

        if (user.Password != passwordHasher.HashPassword(request.Password))
            return Unauthorized();

        var token = tokenGenerator.GenerateToken(user.Id.ToString().Trim());
        var response = new AuthenticationResponse(token, DateTime.Now.AddMinutes(10));
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var isExisting = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email && x.DeletedAt == null);

        if (isExisting != null)
            return BadRequest();

        var user = new User
        {
            Name = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            UserType = UserType.Customer,
            CreatedAt = DateTime.UtcNow
        };

        string hashedPassword = passwordHasher.HashPassword(request.Password);

        user.Password = hashedPassword;

        dataContext.Users.Add(user);
        await dataContext.SaveChangesAsync();

        var token = tokenGenerator.GenerateToken(user.Id.ToString());
        var response = new AuthenticationResponse(token, DateTime.Now.AddMinutes(10));
        return Ok(response);
    }
}