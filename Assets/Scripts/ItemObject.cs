using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;

public class ItemObject : InteractableObject
{
    public List<ItemObject> ConnectedItems;
    public ItemType ItemType;
    private SpriteRenderer _renderer;
    public bool Interactable = true;
    
    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public override void OnActivated(Vector2 mousePosition)
    {
        CharacterController.GoTo(transform.position, () =>
        {
            CharacterController.PickItem(this);
        });
    }

    public void Pick()
    {
        InventoryController.AddItem(new Item
        {
            Type = ItemType,
            Sprite = _renderer.sprite
        });
        
        foreach (var item in ConnectedItems)
        {
            Destroy(item.gameObject);
        }
        
        Destroy(gameObject);
    }
}
