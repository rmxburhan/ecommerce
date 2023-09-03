namespace Ecommerce.Api.Common;

public interface IUploadPath
{
    string UserImageUploadPath();
    string ProductImageUploadPath();
    string StoreImageUploadPath();
}