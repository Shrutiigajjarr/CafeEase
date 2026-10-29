using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEase.Models;

public class Bill
{
    public int Id { get; set; }

    [StringLength(100)]
    [Display(Name = "Customer name")]
    public string? CustomerName { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<BillItem> Items { get; set; } = [];
}
