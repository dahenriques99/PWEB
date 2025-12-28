using System.ComponentModel.DataAnnotations;

namespace RCL.Dtos.Request;

public class ProductRequestDto
{
    public string SupplierId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal FinalPrice { get; set; }

    public int Stock { get; set; }

    public List<int>? CategoryIds { get; set; }
}