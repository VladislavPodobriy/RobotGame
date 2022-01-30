using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell: MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool _drag;
    private bool _active;
    private Camera _camera;
    public Image Image;
    public ItemType ItemType;
    public void Start()
    {
        _camera = Camera.main;
    }
    
    public void Update()
    {
        if (_drag)
            Image.transform.position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void Reset()
    {
        _drag = false;
        Image.transform.position = transform.position;
        InventoryController.Instance.CurrentCell = null;
        Image.rectTransform.sizeDelta = new Vector2(0,0);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _drag = true;
        InventoryController.Instance.CurrentCell = this;
        Image.rectTransform.sizeDelta = Image.sprite.bounds.size / 2;
    }

    public void OnPointerUp(PointerEventData eventData)
    {}
}
