using System.Diagnostics;
using CafeEase.Data;
using CafeEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeEase.Controllers;

public class HomeController(CafeEaseContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.MenuCount = await context.MenuItems.CountAsync();
        ViewBag.BillCount = await context.Bills.CountAsync();
        ViewBag.TodaySales = await context.Bills.Where(b => b.CreatedAt.Date == DateTime.UtcNow.Date)
            .Select(b => (decimal?)b.TotalAmount).SumAsync() ?? 0m;
        ViewBag.RecentBills = await context.Bills.OrderByDescending(b => b.CreatedAt).Take(5).ToListAsync();
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
