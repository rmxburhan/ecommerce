namespace Ecommerce.Api.Common;

public class UploadPath : IUploadPath
{
    private readonly IConfiguration configuration;

    public UploadPath(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string ImageUploadPath()
    {
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", configuration.GetSection("UploadFolder:Image").Value);
        return imagePath;
    }
}