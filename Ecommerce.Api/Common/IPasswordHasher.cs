namespace Ecommerce.Api.Common;

public interface IPasswordHasher
{
    string HashPassword(string password);

}