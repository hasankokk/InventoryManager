using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models.Entities;

public class Tag
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    public ICollection<Item> Items { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}