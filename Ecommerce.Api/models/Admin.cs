namespace Ecommerce.Api.models;

public class Admin
{
    public Guid Id { get; set; }
    public int Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}