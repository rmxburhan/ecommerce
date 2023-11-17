using System.Net.Mime;
using Ecommerce.Api.Common;
using Ecommerce.Api.dto.store;
using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/store/settings")]
public class StoreController : ControllerBase
{
    private readonly ApiDataContext dataContext;
    private readonly IUploadPath uploadPath;

    public StoreController(ApiDataContext dataContext, IUploadPath uploadPath)
    {
        this.dataContext = dataContext;
        this.uploadPath = uploadPath;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetStoreData()
    {
        var store = await dataContext.Stores.FirstOrDefaultAsync();
        if (store == null)
            return NotFound();

        return Ok(store);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateStoreData(UpdateStoreRequest request)
    {
        var store = await dataContext.Stores.FirstOrDefaultAsync();

        if (store == null)
            return NoContent();

        if (!string.IsNullOrEmpty(request.StoreName))
            store.StoreName = request.StoreName;
        if (!string.IsNullOrEmpty(request.Address))
            store.Address = request.Address;
        if (request.Lat.HasValue && request.Lng.HasValue)
        {
            store.Lat = request.Lat;
            store.Lng = request.Lng;
        }

        store.UpdatedAt = DateTime.UtcNow;
        dataContext.Stores.Update(store);
        await dataContext.SaveChangesAsync();
        return Ok(store);
    }

    [Authorize]
    [HttpPost("photo")]
    public async Task<IActionResult> UpdateStorePhoto([FromForm] UpdatePhotoRequest request)
    {
        var store = await dataContext.Stores.FirstOrDefaultAsync();
        if (request.Image != null)
        {
            if (!Directory.Exists(uploadPath.StoreImageUploadPath()))
                Directory.CreateDirectory(uploadPath.StoreImageUploadPath());

            string hashedFilename = Guid.NewGuid().ToString() + "_" + request.Image.FileName;
            string fileName = Path.Combine(uploadPath.StoreImageUploadPath(), hashedFilename);

            request.Image.CopyTo(new FileStream(fileName, FileMode.Create));

            if (store.Image != null)
                if (System.IO.File.Exists(Path.Combine(uploadPath.StoreImageUploadPath(), store.Image)))
                    System.IO.File.Delete(Path.Combine(uploadPath.StoreImageUploadPath(), store.Image));

            store.Image = hashedFilename;
            store.UpdatedAt = DateTime.UtcNow;
            dataContext.Stores.Update(store);
            await dataContext.SaveChangesAsync();
            return Ok(store);
        }
        else
        {
            return BadRequest();
        }
    }
}