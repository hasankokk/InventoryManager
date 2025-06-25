using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Models.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Item
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    public int ItemCount { get; set; }
    public DateOnly? STT { get; set; }
    
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    
    public int ContainerId { get; set; }
    public Container Container { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}