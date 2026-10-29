using CafeEase.Data;
using CafeEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeEase.Controllers;

public class MenuItemsController(CafeEaseContext context) : Controller
{
    public async Task<IActionResult> Index(string? search, string? category)
    {
        var query = context.MenuItems.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.Name.Contains(search) || x.Category.Contains(search));
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(x => x.Category == category);
        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Categories = await context.MenuItems.Select(x => x.Category).Distinct().OrderBy(x => x).ToListAsync();
        return View(await query.OrderBy(x => x.Category).ThenBy(x => x.Name).ToListAsync());
    }

    public IActionResult Create() => View(new MenuItem());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItem item)
    {
        if (!ModelState.IsValid) return View(item);
        item.CreatedAt = DateTime.UtcNow;
        context.MenuItems.Add(item);
        await context.SaveChangesAsync();
        TempData["Success"] = "Menu item added successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await context.MenuItems.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MenuItem item)
    {
        if (id != item.Id) return BadRequest();
        if (!ModelState.IsValid) return View(item);
        var existing = await context.MenuItems.FindAsync(id);
        if (existing is null) return NotFound();
        existing.Name = item.Name;
        existing.Category = item.Category;
        existing.Price = item.Price;
        existing.AvailableQuantity = item.AvailableQuantity;
        await context.SaveChangesAsync();
        TempData["Success"] = "Menu item updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await context.MenuItems.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await context.MenuItems.FindAsync(id);
        if (item is null) return NotFound();
        if (await context.BillItems.AnyAsync(x => x.MenuItemId == id))
        {
            TempData["Error"] = "This item is linked to an existing bill and cannot be deleted. Set its quantity to zero instead.";
            return RedirectToAction(nameof(Index));
        }
        context.MenuItems.Remove(item);
        await context.SaveChangesAsync();
        TempData["Success"] = "Menu item deleted.";
        return RedirectToAction(nameof(Index));
    }
}
