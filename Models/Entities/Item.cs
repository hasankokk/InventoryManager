using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models.Entities;

public class Item
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    public int ItemCount { get; set; }
    public DateOnly? STT { get; set; }
    public int ContainerId { get; set; }
    public Container Container { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}