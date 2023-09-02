using Ecommerce.Api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls;

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
}