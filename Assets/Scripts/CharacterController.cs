using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Pixelplacement;
using UnityEngine;

public class CharacterController : Singleton<CharacterController>
{
    public Animator Anim;
    public AudioSource AudioSource;
    
    public SpriteRenderer Renderer;
    public float Speed;
    
    private Coroutine _movingRoutine;

    private ItemObject _itemToPick;
    private Crap _crapToPick;
    private Action _throwCoinCallback;
    private Action _throwBoomerangCallback;

    public List<AudioClip> StepSounds;
    public AudioClip NoAccessSound;
    public ItemObject NewBoomerang;
    
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

    public static void PickCrap(Crap crap)
    {
        StopMoving();
        Instance.Anim.Play("RoboPickCrap");
        Instance._crapToPick = crap;
    }

    public void  OnPickCrapAnimationEnd()
    {
        if (_crapToPick != null)
        {
            _crapToPick.Pick();
        }
    }

    public static void ThrowCoin(Action onDone)
    {
        Instance.Anim.Play("RoboThrowCoin");
        Instance._throwCoinCallback = onDone;
    }

    public void OnThrowCoinAnimationEnd()
    {
        Instance._throwCoinCallback?.Invoke();
    }

    public static void ThrowBoomerang(Action onDone)
    {
        Instance.Anim.Play("RoboThrowBoomerang");
        Instance._throwBoomerangCallback = onDone;
        Instance.Renderer.flipX = true;
    }
    
    public void OnThrowBoomerangAnimationEnd()
    {
        Instance._throwBoomerangCallback?.Invoke();
        DialogsController.Show(DialogType.Boomerang);
        InteractionController.Instance.BoomerangLost = true;
        NewBoomerang.gameObject.SetActive(true);
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
            while (Vector2.Distance(waypoint, transform.position) > 0.5f)
            {
                var speed = Time.deltaTime * Speed;
                var pos = transform.position;
                var newPosition = Vector2.MoveTowards(transform.position, waypoint, speed);
                transform.position = newPosition;
                yield return new WaitForFixedUpdate();
            }
        }
        Anim.SetBool("walking", false);
        onDone?.Invoke();
    }

    public void PlayStepSound(int index)
    {
        AudioSource.PlayOneShot(StepSounds[index]);
    }

    public void PlayNoAccessSound()
    {
        AudioSource.PlayOneShot(NoAccessSound);
    }
}
