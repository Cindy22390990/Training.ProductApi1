using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Training.ProductApi1.Models;

public class Bom
{
    [Key] 
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ProductId { get; set; } = string.Empty;
   
    [Required]
    [StringLength (50)]
    public string MaterialId {  get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public Product? Product { get; set; }
    [JsonIgnore]
    public Material? Material { get; set; } 

}
