namespace Training.ProductApi1.Models.DTOs;

public class BomResultDto
{
    public int Id { get; set; }

    public string ProductId { get; set; } = string.Empty;

    public string MaterialId { get; set; } = string.Empty;

    public int Quantity { get; set; }
}