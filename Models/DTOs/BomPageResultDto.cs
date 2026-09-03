namespace Training.ProductApi1.Models.DTOs
{
    public class BomPageResultDto
    {
        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<Bom> Data { get; set; } = new();
    }
}
