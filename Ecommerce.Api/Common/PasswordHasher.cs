using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Org.BouncyCastle.Utilities;

namespace Ecommerce.Api.Common;

public class PasswordHasher : IPasswordHasher
{
    private readonly IConfiguration configuration;

    public PasswordHasher(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string HashPassword(string password)
    {
        using (var hmac = SHA256.Create())
        {
            byte[] bytesPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < bytesPassword.Length; i++)
            {
                stringBuilder.Append(bytesPassword[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}