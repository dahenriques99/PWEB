namespace RCL.Dtos.Request;

public class ProductQuery
{
    public List<int>? CategoryIds { get; set; }
    public bool MatchAllCategories { get; set; } = false;
}