using InventoryManager.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
   public DbSet<Item> Items { get; set; }
   public DbSet<Container> Containers { get; set; }
   public DbSet<Location> Locations { get; set; }
   public DbSet<Tag> Tags { get; set; }
}
