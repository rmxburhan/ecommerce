using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Api.Common;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateToken(string identifier)
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SigningKey").Value));
        var signingCredential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);

        var claim = new[] {
            new Claim(ClaimTypes.NameIdentifier, identifier.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration.GetSection("JwtSettings:Issuer").Value,
            audience: configuration.GetSection("JwtSettings:Audience").Value,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signingCredential,
            claims: claim
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}