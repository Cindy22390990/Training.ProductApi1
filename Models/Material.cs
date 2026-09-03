using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Training.ProductApi1.Models;

public class Material
{
    [Key]
    [StringLength(50)]
    public string MaterialId { get; set; } = string.Empty;
    
    [Required]
    [StringLength (100)]
    public string Name { get; set; }= string.Empty;
    public int Stock { get; set; } = 0;
    public DateTime CreatedAt { get; set; }= DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public ICollection<Bom> Boms { get; set; } = new List<Bom>();

}
