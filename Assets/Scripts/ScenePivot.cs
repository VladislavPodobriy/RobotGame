using DefaultNamespace;
using Enums;
using UnityEngine;

public class ScenePivot : InteractableObject
{
    public ItemType ItemType;
    private SpriteRenderer _renderer;
    public SpriteRenderer Shadow;
    public Vector2 Diff;
    public bool Activated = true;
    
    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Diff = Shadow.transform.position - transform.position;
        
        if (ScreenController.Instance.ActivatedItems.Contains(ItemType))
        {
            Activate();
        }
    }
    
    public override void OnActivated(Vector2 mousePosition)
    {
        
    }

    public override bool OnUseItem(ItemType itemType)
    {
        if (itemType != ItemType)
            return false;

        Activate();
        ScreenController.Instance.ActivatedItems.Add(itemType);
        
        return true;
    }

    public void Activate()
    {
        Activated = true;
        _renderer.enabled = true;
        Shadow.enabled = true;
        Shadow.transform.position = transform.position + (Vector3)Diff;
        Shadow.gameObject.SetActive(true);
        Interactable = false;
    }
}
