namespace Ecommerce.Api.dto.admin;

public record LoginAdminRequest(
    string Email,
    string Password);