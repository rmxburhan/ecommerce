using System.Security.Claims;
using Ecommerce.Api.Common;
using Ecommerce.Api.dto.user;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApiDataContext dataContext;
    private readonly IUploadPath uploadPath;

    public UserController(ApiDataContext dataContext, IUploadPath uploadPath)
    {
        this.dataContext = dataContext;
        this.uploadPath = uploadPath;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await dataContext.Users.FindAsync(int.Parse(id));

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile(UpdateProfileRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id));

        if (user == null)
            return Unauthorized();

        if (!string.IsNullOrEmpty(request.Name))
            user.Name = request.Name;

        user.UpdatedAt = DateTime.UtcNow;

        dataContext.Users.Update(user);
        await dataContext.SaveChangesAsync();

        return Ok(user);
    }
    [HttpPut("me/photo")]
    public async Task<IActionResult> UpdateMyProfilePicture(UpdateProfileImageRequest request)
    {
        ClaimsPrincipal claims = HttpContext.User;
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dataContext.Users.FindAsync(int.Parse(id));

        if (user == null)
            return NotFound();

        if (request.Photo != null)
        {
            if (Directory.Exists(uploadPath.UserImageUploadPath()))
                Directory.CreateDirectory(uploadPath.UserImageUploadPath());

            string hashedFilename = Guid.NewGuid().ToString() + "_" + request.Photo.FileName;
            string fileName = Path.Combine(uploadPath.UserImageUploadPath(), hashedFilename);

            request.Photo.CopyTo(new FileStream(fileName, FileMode.Create));
            if (user.Photo != null)
                if (System.IO.File.Exists(Path.Combine(uploadPath.UserImageUploadPath(), user.Photo)))
                    System.IO.File.Delete(Path.Combine(uploadPath.UserImageUploadPath(), user.PhoneNumber));

            user.Photo = hashedFilename;
            user.UpdatedAt = DateTime.UtcNow;

            dataContext.Users.Update(user);
            await dataContext.SaveChangesAsync();
            return Ok(user);
        }
        else
        {
            return BadRequest();
        }
    }
}