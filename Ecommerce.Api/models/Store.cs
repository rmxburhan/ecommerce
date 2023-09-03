using System;
namespace Ecommerce.Api.models;

public class Store
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string? Image { get; set; }
    public string Address { get; set; }
    public Double? Lat { get; set; }
    public Double? Lng { get; set; }
    public DateTime UpdatedAt { get; set; }
}