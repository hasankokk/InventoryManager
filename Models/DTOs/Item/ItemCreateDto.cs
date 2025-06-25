using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Models.DTOs.Item;

[Index(nameof(Name), IsUnique = true)]
public class ItemCreateDto
{
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [Range(1, int.MaxValue)]
    public int ItemCount { get; set; }
    public DateOnly? STT { get; set; }
    
    public string? UserId { get; set; }
    public int ContainerId { get; set; }
    public List<int> Tags { get; set; }
}