using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pixelplacement;
using UnityEngine;

public class CharacterController : Singleton<CharacterController>
{
    public Animator Anim;
    public SpriteRenderer Renderer;
    public float Speed;
    
    private Coroutine _movingRoutine;

    private ItemObject _itemToPick;
    
    public static void PickItem(ItemObject item)
    {
        StopMoving();
        Instance.Anim.Play("RoboPick");
        Instance._itemToPick = item;
    }

    public void OnPickAnimationEnd()
    {
        if (_itemToPick != null)
        {
            _itemToPick.Pick();
        }
    }
    
    public static void GoTo(Vector2 target, Action onDone = null)
    {
        var waypoints = PathManager.GetPath(Instance.transform.position, target);
        if (waypoints.Any())
        {
            StopMoving();
            Instance._movingRoutine = Instance.StartCoroutine(Instance.MovingRoutine(waypoints, onDone));
        }
    }

    public static void StopMoving()
    {
        if (Instance._movingRoutine != null)
            Instance.StopCoroutine(Instance._movingRoutine);
    }

    private IEnumerator MovingRoutine(List<Vector2> waypoints, Action onDone = null)
    {
        Anim.SetBool("walking", true);
        foreach (var waypoint in waypoints)
        {
            Renderer.flipX = waypoint.x < transform.position.x;
            
            while (Vector2.Distance(waypoint, transform.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoint, Time.deltaTime*Speed);
                yield return new WaitForFixedUpdate();
            }
        }
        Anim.SetBool("walking", false);
        onDone?.Invoke();
    }
}
