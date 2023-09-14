namespace Ecommerce.Api.models;

public class RepliedReview
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public Guid AdminId { get; set; }
    public virtual Admin Admin { get; set; }
    public virtual Review? Review { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}