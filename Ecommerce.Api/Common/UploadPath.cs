namespace Ecommerce.Api.Common;

public class UploadPath : IUploadPath
{
    private readonly IConfiguration configuration;

    public UploadPath(IConfiguration configuration)
    {
        this.configuration = configuration;
    }


    public string ProductImageUploadPath()
    {
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "products", configuration.GetSection("UploadFolder:Image").Value);
        return imagePath;
    }

    public string StoreImageUploadPath()
    {
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "store");
        return imagePath;
    }

    public string UserImageUploadPath()
    {
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", configuration.GetSection("UploadFolder:Image").Value);
        return imagePath;
    }
}