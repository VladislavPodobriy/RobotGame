using Enums;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public InteractableObjectType Type;
    public bool Interactable = true;
    
    public abstract void OnActivated(Vector2 mousePosition);

    public virtual bool OnUseItem(ItemType itemType)
    {
        return false;
    }

    public virtual void OnHover(ItemType type)
    {}

    public virtual void OnHoverEnd()
    { }
}
