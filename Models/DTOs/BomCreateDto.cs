namespace Training.ProductApi1.Models.DTOs
{
    public class BomCreateDto
    {
        public string ProductId { get; set; } = string.Empty;

        public string MaterialId { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
