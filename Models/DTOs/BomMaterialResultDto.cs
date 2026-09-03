namespace Training.ProductApi1.Models.DTOs
{
    public class BomMaterialResultDto
    {
        public string ProductId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public int Stock { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
