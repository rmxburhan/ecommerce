namespace Ecommerce.Api.dto.authentication;

public record AuthenticationResponse(
    string Token,
    DateTime ExpiredTime
);