namespace Ecommerce.Api.dto.transaction;

public record AddTransactionRequest(
    Guid AddressId,
    Guid[] CartId);