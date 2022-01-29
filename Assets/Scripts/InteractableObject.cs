using Enums;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public InteractableObjectType Type;
    
    public abstract void OnActivated(Vector2 mousePosition);
}
