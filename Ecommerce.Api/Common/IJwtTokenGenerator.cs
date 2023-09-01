namespace Ecommerce.Api.Common;

public interface IJwtTokenGenerator
{
    string GenerateToken(string identifier);
}