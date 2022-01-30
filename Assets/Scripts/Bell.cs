using System.Collections;
using Enums;
using UnityEngine;

public class Bell : ItemObject
{
    public Transform Pivot;
    public Transform FallPivot;
    public bool Pickable;
    
    public override void OnActivated(Vector2 mousePosition)
    {
        CharacterController.GoTo(transform.position, () =>
        {
            if (Pickable)
            {
                CharacterController.PickItem(this);
            }
            else
            {
               DialogsController.Show(DialogType.Bell); 
            }
        });
    }

    public override bool OnUseItem(ItemType itemType)
    {
        if (itemType != ItemType.Boomerang)
            return false;
        
        InteractionController.BlockInteraction(true);
        CharacterController.GoTo(Pivot.position, () =>
        {
            CharacterController.ThrowBoomerang(() =>
            {
                InteractionController.BlockInteraction(false);
                StartCoroutine(Fall());
            });
        });

        return true;
    }
    
    private IEnumerator Fall()
    {
        while (Vector2.Distance(transform.position, FallPivot.position) > 0.1)
        {
            transform.position = Vector2.MoveTowards(transform.position, FallPivot.position, Time.deltaTime*5);
            yield return new WaitForFixedUpdate();
        }
        Pickable = true;
    }
}
