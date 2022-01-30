using Models;
using Pixelplacement;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public InventoryCell CurrentCell;
    public RectTransform Grid;
    public InventoryCell CellPrefab;
    
    public static void AddItem(Item item)
    {
        var cell = Instantiate(Instance.CellPrefab, Instance.Grid);
        cell.Image.sprite = item.Sprite;
        cell.ItemType = item.Type;
        Instance.Grid.sizeDelta = new Vector2(Instance.transform.childCount * 50, Instance.Grid.sizeDelta.y);
    }

    public static void ResetCurrentCell()
    {
        if (Instance.CurrentCell != null)
            Instance.CurrentCell.Reset();
    }

    public static void DeleteCurrentCell()
    {
        if (Instance.CurrentCell != null)
        {
            Instance.CurrentCell.Delete();
            Instance.Grid.sizeDelta = new Vector2(Instance.transform.childCount * 50, Instance.Grid.sizeDelta.y);
        }
    }
}
