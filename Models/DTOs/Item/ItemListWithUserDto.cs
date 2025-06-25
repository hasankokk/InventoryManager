namespace InventoryManager.Models.DTOs.Item;

public class ItemListWithUserDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateOnly? STT { get; set; }
    public int ItemCount { get; set; }
    public string Username { get; set; }
}