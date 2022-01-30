using System.Linq;
using Enums;
using Pixelplacement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionController : Singleton<InteractionController>
{
    private Camera _camera;
    private InteractableObject _hoverObject;
    
    public Texture2D PickCursorTexture;
    public Texture2D DoorCursorTexture;
    public Texture2D ClosedDoorCursorTexture;
    public Texture2D WalkingCursorTexture;
    public Texture2D SpeakCursorTexture;
    public Transform PaperWindow;
    public bool ShowPaperDialog;
    public bool BoomerangLost;
    public LocationDoor SceneLocationDoor;
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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        
        if (_blocked)
            return;
        
        if (Input.GetMouseButtonUp(0))
        {
            if (InventoryController.Instance.CurrentCell != null &&
                InventoryController.Instance.CurrentCell.ItemType == ItemType.Paper)
            {
                ShowPaper();
            }
            else if (_hoverObject != null)
            {
                if (InventoryController.Instance.CurrentCell != null)
                {
                    var use = _hoverObject.OnUseItem(InventoryController.Instance.CurrentCell.ItemType);
                    if (use)
                        InventoryController.DeleteCurrentCell();
                }
                else
                {
                    _hoverObject.OnActivated(_camera.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            InventoryController.ResetCurrentCell();
        }
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        var interactableObject = hits
            .Select(x => x.collider.GetComponent<InteractableObject>())
            .Where(x => x != null && x.Interactable)
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

            _hoverObject = interactableObject;
            
            if (InventoryController.Instance.CurrentCell == null)
            {
                Cursor.SetCursor(cursorTexture, new Vector2(32, 32), CursorMode.ForceSoftware);
                _hoverObject.OnHoverEnd();
            }
            else
            {
                _hoverObject.OnHover(InventoryController.Instance.CurrentCell.ItemType);
            }
            
            return;
        }
        
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        _hoverObject = null;
    }

    public static void ShowPaper()
    {
        Instance.PaperWindow.gameObject.SetActive(true);
        BlockInteraction(true);
        Time.timeScale = 0;
    }
    
    public void HidePaper()
    {
        BlockInteraction(false);
        Instance.PaperWindow.gameObject.SetActive(false);
        Time.timeScale = 1;
        if (ShowPaperDialog)
        {
            DialogsController.Show(DialogType.Paper);
            ShowPaperDialog = false;
        }
    }
}
