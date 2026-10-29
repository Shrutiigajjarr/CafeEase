using CafeEase.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeEase.Data;

public class CafeEaseContext(DbContextOptions<CafeEaseContext> options) : DbContext(options)
{
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillItem> BillItems => Set<BillItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<MenuItem>().Property(x => x.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Bill>().Property(x => x.TotalAmount).HasPrecision(10, 2);
        modelBuilder.Entity<BillItem>().Property(x => x.UnitPrice).HasPrecision(10, 2);
        modelBuilder.Entity<BillItem>().Property(x => x.LineTotal).HasPrecision(10, 2);
        modelBuilder.Entity<BillItem>()
            .HasOne(x => x.MenuItem).WithMany()
            .HasForeignKey(x => x.MenuItemId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<BillItem>()
            .HasOne(x => x.Bill).WithMany(x => x.Items)
            .HasForeignKey(x => x.BillId).OnDelete(DeleteBehavior.Cascade);
    }
}
