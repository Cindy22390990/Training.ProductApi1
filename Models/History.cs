using System.ComponentModel.DataAnnotations;
namespace Training.ProductApi1.Models;

public class History
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string TargetId { get; set; } = string.Empty;
    [Required]
    [StringLength(20)]
    public string Category {  get; set; }= string.Empty;
    [Required,StringLength(20)]
    public string Action {  get; set; } = string.Empty;
    [Required]
    [StringLength (20)]
    public string Status {  get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }= DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}
