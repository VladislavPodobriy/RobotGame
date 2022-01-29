using UnityEngine;

public class LocationDoor : InteractableObject
{
    public LocationDoor ConnectedDoor;
    public GameObject Location;
    public Transform Pivot;
    public bool Closed;
    
    public override void OnActivated(Vector2 mousePosition)
    {
        if (Closed)
            return;
        
        CharacterController.GoTo(transform.position, () =>
        {
            LocationController.Instance.EnterLocation(ConnectedDoor);
        });
    }
}
