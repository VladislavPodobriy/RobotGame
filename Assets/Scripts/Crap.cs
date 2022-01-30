using System.Collections.Generic;
using System.Linq;
using Enums;
using Models;

using UnityEngine;

public class Crap : InteractableObject
{
    public List<Item> Items = new List<Item>();

    public override void OnActivated(Vector2 mousePosition)
    {
        CharacterController.GoTo(transform.position, () =>
        {
            if (InteractionController.Instance.SceneLocationDoor.Closed && 
                !Items.Any(x => x.Type == ItemType.Coin || x.Type == ItemType.Paper))
            {
                DialogsController.Show(DialogType.ItIsACrap);
            }
            else if (Items.Any())
            {
                CharacterController.PickCrap(this);
            }
            else
            {
                DialogsController.Show(DialogType.CrapIsEmpty);
            }
        });
    }

    public void Pick()
    {
        if (Items.Any())
        {
            foreach (var item in Items)
            {
                InventoryController.AddItem(item);
            }

            Items = new List<Item>();
        }
    }
}
