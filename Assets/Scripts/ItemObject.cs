using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;

public class ItemObject : InteractableObject
{
    public List<ItemObject> ConnectedItems;
    public ItemType ItemType;
    private SpriteRenderer _renderer;
    
    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public override void OnActivated(Vector2 mousePosition)
    {
        CharacterController.GoTo(transform.position, () =>
        {
            if (InteractionController.Instance.SceneLocationDoor.Closed && 
                ItemType != ItemType.Coin && ItemType != ItemType.Paper)
            {
                DialogsController.Show(DialogType.ItIsACrap);
            }
            else
            {
                CharacterController.PickItem(this);
            }
        });
    }

    public void Pick()
    {
        InventoryController.AddItem(new Item
        {
            Type = ItemType,
            Sprite = _renderer.sprite
        });

        if (ItemType == ItemType.Paper)
        {
            InteractionController.ShowPaper();
            InteractionController.Instance.SceneLocationDoor.Closed = false;
            InteractionController.Instance.ShowPaperDialog = true;
        }

        if (ItemType == ItemType.Boomerang && InteractionController.Instance.BoomerangLost)
        {
            DialogsController.Show(DialogType.BoomerangReturn);
        }
        
        foreach (var item in ConnectedItems)
        {
            Destroy(item.gameObject);
        }
        
        Destroy(gameObject);
    }

    public void ToggleInteractable(bool value)
    {
        Interactable = true;
        foreach (var item in ConnectedItems)
        {
            item.Interactable = true;
        }
    }

    public Sprite GetSprite()
    {
        return _renderer.sprite;
    }
}
