using UnityEngine;

public class Ground : InteractableObject
{
    public override void OnActivated(Vector2 mousePosition)
    {
        CharacterController.GoTo(mousePosition);
    }
}
