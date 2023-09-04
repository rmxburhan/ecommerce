namespace Ecommerce.Api.dto.order;

public record AddOrderRequest(
    int Total,
    int[] ChartsId,
    int AddressId
);