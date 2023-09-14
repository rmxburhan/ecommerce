namespace Ecommerce.Api.models;

public class Review
{
    public Guid Id { get; set; }
    public int Ratings { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? RepliedReviewId { get; set; }
    public virtual RepliedReview? RepliedReview { get; set; }
    public Guid OrderDetailId { get; set; }
    public virtual OrderDetail OrderDetail { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}