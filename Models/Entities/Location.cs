using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models.Entities;

public class Location
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    public ICollection<Container> Containers { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}