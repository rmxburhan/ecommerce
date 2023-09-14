using System.Text.Json.Serialization;

namespace Ecommerce.Api.models;
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
