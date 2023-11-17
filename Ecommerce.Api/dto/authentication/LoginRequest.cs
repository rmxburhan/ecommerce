namespace Ecommerce.Api.dto.authentication;

public record LoginRequest(
    string Email, 
    string Password);