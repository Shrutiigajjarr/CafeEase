using System.ComponentModel.DataAnnotations;

namespace CafeEase.Models;

public class CreateBillViewModel
{
    [StringLength(100)]
    [Display(Name = "Customer name (optional)")]
    public string? CustomerName { get; set; }

    [Required]
    public List<BillLineInput> Lines { get; set; } = [];
}

public class BillLineInput
{
    public int MenuItemId { get; set; }
    [Range(0, 100000)]
    public int Quantity { get; set; }
}
