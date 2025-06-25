using InventoryManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
   public DbSet<Item> Items { get; set; }
   public DbSet<Container> Containers { get; set; }
}