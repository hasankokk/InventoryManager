using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models.DTOs.Item;

public class ItemUpdateDto
{   
    [MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    public int? ItemCount { get; set; }
    public DateOnly? STT { get; set; }
    public int? ContainerId { get; set; }
    public bool IsAdded { get; set; } = true;
    public List<int?> Tags { get; set; }
}