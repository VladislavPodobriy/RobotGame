using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class Screen : InteractableObject
{
    public List<ScenePivot> Pivots;
    
    public override void OnActivated(Vector2 mousePosition)
    {}

    public override void OnHover(ItemType type)
    {
        var pivot = Pivots.FirstOrDefault(x => x.ItemType == type);
        if (pivot != null)
        {
            pivot.Shadow.gameObject.SetActive(true);
            pivot.Shadow.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + pivot.Diff;
        }
    }

    public override void OnHoverEnd()
    {
        foreach (var scenePivot in Pivots)
        {
            if (!scenePivot.Activated)
                scenePivot.Shadow.gameObject.SetActive(false);
        }
    }
}
