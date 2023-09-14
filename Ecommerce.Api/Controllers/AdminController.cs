using Ecommerce.Api.Common;
using Ecommerce.Api.dto.admin;
using Ecommerce.Api.dto.authentication;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/auth/admin")]
public class AdminController : ControllerBase
{
    private readonly ApiDataContext dataContext;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtTokenGenerator tokenGenerator;

    public AdminController(ApiDataContext dataContext, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
    {
        this.dataContext = dataContext;
        this.passwordHasher = passwordHasher;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin(LoginAdminRequest request)
    {
        var user = await dataContext.Admins.FirstOrDefaultAsync(x => x.Email == request.Email && x.DeletedAt == null);

        if (user == null)
            return NotFound();

        if (user.Password != passwordHasher.HashPassword(request.Password))
            return Unauthorized();

        var token = tokenGenerator.GenerateToken(user.Id.ToString());

        var result = new AuthenticationResponse(
            token,
            DateTime.Now.AddMinutes(10)
        );

        return Ok(result);
    }
}