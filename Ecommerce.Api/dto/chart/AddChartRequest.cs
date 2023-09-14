namespace Ecommerce.Api.dto.chart;

public class AddChartRequest
{
    public int Qty { get; set; } = 1;
    public string Notes { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
}