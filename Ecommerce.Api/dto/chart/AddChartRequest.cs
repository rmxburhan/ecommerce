namespace Ecommerce.Api.dto.chart;

public record AddChartRequest(
    Guid ProductId,
    int Qty = 1,
    string Notes = ""
);
