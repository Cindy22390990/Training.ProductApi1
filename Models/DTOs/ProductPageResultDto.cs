namespace Training.ProductApi1.Models.DTOs;

public class ProductPageResultDto
{
    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public IEnumerable<Product> Data { get; set; } = new List<Product>();
}
