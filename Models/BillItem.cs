using System.ComponentModel.DataAnnotations.Schema;

namespace CafeEase.Models;

public class BillItem
{
    public int Id { get; set; }
    public int BillId { get; set; }
    public Bill Bill { get; set; } = null!;
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;
    public string ItemName { get; set; } = "";
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal LineTotal { get; set; }
}
