using System;
namespace Ecommerce.Api.models;

public class Store
{
    public Guid Id { get; set; }
    public string StoreName { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Address { get; set; }
    public Double? Lat { get; set; }
    public Double? Lng { get; set; }
    public DateTime? UpdatedAt { get; set; }
}