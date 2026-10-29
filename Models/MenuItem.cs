using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEase.Models;

public class MenuItem
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Item name")]
    public string Name { get; set; } = "";

    [Required, StringLength(60)]
    public string Category { get; set; } = "";

    [Range(typeof(decimal), "0.01", "999999")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Available quantity")]
    public int AvailableQuantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
