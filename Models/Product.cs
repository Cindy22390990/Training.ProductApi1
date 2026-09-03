using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Training.ProductApi1.Models;

public class Product
{
    [Key]//Primary Key
    [StringLength(50)]//NVARCHAR(50)
    public string ProductId { get; set; }= string.Empty;

    [Required] 
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; } = 0;
    [Column(TypeName ="decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0;
    public DateTime CreatedAt { get; set; }= DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public ICollection<Bom> Boms { get; set; } = new List<Bom>();
}
