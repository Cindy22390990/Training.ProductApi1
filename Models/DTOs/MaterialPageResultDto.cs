using Training.ProductApi1.Models;

namespace Training.ProductApi1.Models.DTOs;

public class MaterialPageResultDto
{
    public int TotalCount { get; set; }

    public int TotalPages { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public List<Material> Data { get; set; } = new();
}
