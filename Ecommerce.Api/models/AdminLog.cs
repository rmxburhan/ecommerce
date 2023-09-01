namespace Ecommerce.Api.models;

public class AdminLog
{
    public int Id { get; set; }
    public string Activity { get; set; }
    public int AdminId { get; set; }
    public virtual Admin Admin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}