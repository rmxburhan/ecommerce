namespace Ecommerce.Api.models;

public class AdminLog
{
    public Guid Id { get; set; }
    public string Activity { get; set; }
    public Guid AdminId { get; set; }
    public virtual Admin Admin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}