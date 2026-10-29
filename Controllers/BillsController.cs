using CafeEase.Data;
using CafeEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeEase.Controllers;

public class BillsController(CafeEaseContext context) : Controller
{
    public async Task<IActionResult> Index()
        => View(await context.Bills.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var bill = await context.Bills.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
        if (bill is null) return NotFound();
        return View(bill);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.MenuItems = await context.MenuItems.Where(x => x.AvailableQuantity > 0)
            .OrderBy(x => x.Name).ToListAsync();
        return View(new CreateBillViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBillViewModel model)
    {
        var requested = model.Lines.Where(x => x.Quantity > 0).ToList();
        if (requested.Count == 0)
            ModelState.AddModelError("", "Select at least one menu item and enter a quantity.");
        if (!ModelState.IsValid)
        {
            ViewBag.MenuItems = await context.MenuItems.Where(x => x.AvailableQuantity > 0).OrderBy(x => x.Name).ToListAsync();
            return View(model);
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        var bill = new Bill { CustomerName = string.IsNullOrWhiteSpace(model.CustomerName) ? "Walk-in customer" : model.CustomerName.Trim(), CreatedAt = DateTime.UtcNow };
        foreach (var line in requested)
        {
            var item = await context.MenuItems.FirstOrDefaultAsync(x => x.Id == line.MenuItemId);
            if (item is null || item.AvailableQuantity < line.Quantity)
            {
                ModelState.AddModelError("", $"Not enough stock available for {item?.Name ?? "one selected item"}. Please review quantities.");
                await transaction.RollbackAsync();
                ViewBag.MenuItems = await context.MenuItems.Where(x => x.AvailableQuantity > 0).OrderBy(x => x.Name).ToListAsync();
                return View(model);
            }
            item.AvailableQuantity -= line.Quantity;
            bill.Items.Add(new BillItem {
                MenuItemId = item.Id, ItemName = item.Name, Quantity = line.Quantity,
                UnitPrice = item.Price, LineTotal = item.Price * line.Quantity
            });
        }
        bill.TotalAmount = bill.Items.Sum(x => x.LineTotal);
        context.Bills.Add(bill);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        TempData["Success"] = "Bill created and stock updated.";
        return RedirectToAction(nameof(Details), new { id = bill.Id });
    }
}
