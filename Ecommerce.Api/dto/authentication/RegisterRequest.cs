namespace Ecommerce.Api.dto.authentication;

public record RegisterRequest(
    string Name,
    string PhoneNumber,
    string Email,
    string Password);
