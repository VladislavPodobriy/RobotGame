using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class LocationController : Singleton<LocationController>
{
    public CharacterController Character;
    public List<GameObject> Locations;
    public GameObject ActiveLocation;

    public void Start()
    {
        foreach (var location in Locations)
        {
            location.transform.position = Vector3.zero;
            location.SetActive(false);
        }
        ActiveLocation.SetActive(true);
        PathManager.UpdateWaypoints();
    }
    
    public void EnterLocation(LocationDoor door)
    {
        ActiveLocation.SetActive(false);
        Character.transform.position = door.Pivot.transform.position;
        door.Location.SetActive(true);
        ActiveLocation = door.Location;
        PathManager.UpdateWaypoints();
    }
}
