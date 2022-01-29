using System.Collections.Generic;
using Models;
using Pixelplacement;

public class InventoryController : Singleton<InventoryController>
{
    public List<Item> Items = new List<Item>();

    public static void AddItem(Item item)
    {
        Instance.Items.Add(item);
    }
}
