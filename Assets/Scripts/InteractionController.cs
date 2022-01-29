using System.Linq;
using Enums;
using Pixelplacement;
using UnityEngine;

public class InteractionController : Singleton<InteractionController>
{
    private Camera _camera;
    private InteractableObject _hoverObject;
    
    public Texture2D PickCursorTexture;
    public Texture2D DoorCursorTexture;
    public Texture2D ClosedDoorCursorTexture;
    public Texture2D WalkingCursorTexture;
    public Texture2D SpeakCursorTexture;

    private bool _blocked;
    
    void Start()
    {
        _camera = Camera.main;
    }

    public static void BlockInteraction(bool value)
    {
        Instance._blocked = value;
        if (value)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Instance._hoverObject = null;
        }
    }
    
    void Update()
    {
        if (_blocked)
            return;
        
        if (Input.GetMouseButtonUp(0) && _hoverObject != null)
        {
            _hoverObject.OnActivated(_camera.ScreenToWorldPoint(Input.mousePosition));
        }
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        var interactableObject = hits
            .Select(x => x.collider.GetComponent<InteractableObject>())
            .Where(x => x != null)
            .OrderByDescending(x => x.Type)
            .FirstOrDefault();
        
        if (interactableObject != null)
        {
            Texture2D cursorTexture = null;
            switch (interactableObject.Type)
            {
                case InteractableObjectType.LocationDoor:
                    var door = (LocationDoor) interactableObject;
                    cursorTexture = door.Closed ? ClosedDoorCursorTexture : DoorCursorTexture;
                    break;
                case InteractableObjectType.Ground:
                    cursorTexture = WalkingCursorTexture;
                    break;
                case InteractableObjectType.Item:
                    cursorTexture = PickCursorTexture;
                    break;
                case InteractableObjectType.SpeakObject:
                    cursorTexture = SpeakCursorTexture;
                    break;
            }
            Cursor.SetCursor(cursorTexture, new Vector2(32, 32), CursorMode.ForceSoftware); 
            _hoverObject = interactableObject;
            return;
        }
        
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        _hoverObject = null;
    }
}
