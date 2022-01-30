using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GridyRobot : InteractableObject
{
    private Animator _anim;
    public Transform Pivot;
    public ItemObject Chair;
    public Transform RunPivot;
    public GridyRobot ConnectedRobot;
    public bool CanTakeCoin;
    public AudioSource AudioSource;
    public AudioClip SpeakClip;
    public List<AudioClip> RunClips;
    
    public void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Robot");
        if (other.CompareTag("Player"))
        {
            _anim.Play("GridyRobotSpeak");
            InteractionController.BlockInteraction(true);
            CharacterController.GoTo(Pivot.position, () =>
            {
                InteractionController.BlockInteraction(false);
            });
        }
    }

    public override void OnActivated(Vector2 mousePosition)
    {
        CharacterController.GoTo(Pivot.position, () =>
        {
            _anim.Play("GridyRobotSpeak");
        });
    }

    public void OnSpeakAnimationEnd()
    {
        DialogsController.Show(DialogType.Gridy);
    }
    
    public override bool OnUseItem(ItemType itemType)
    {
        if (itemType != ItemType.Coin || !CanTakeCoin)
            return false;
        
        InteractionController.BlockInteraction(true);
        CharacterController.GoTo(Pivot.position, () =>
        {
            CharacterController.ThrowCoin(() =>
            {
                Chair.ToggleInteractable(true);
                Interactable = false;
                InteractionController.BlockInteraction(false);
                StartCoroutine(Run());
            });
        });

        return true;
    }

    private IEnumerator Run()
    {
        _anim.Play("GridyRobotRun");
        while (Vector2.Distance(transform.position, RunPivot.position) > 0.5)
        {
            transform.position = Vector2.MoveTowards(transform.position, RunPivot.position, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
        Destroy(ConnectedRobot.gameObject);
    }

    public void PlaySpeakSound()
    {
        AudioSource.PlayOneShot(SpeakClip);
    }
    
    public void StopSpeak()
    {
        AudioSource.Stop();
    }

    public void PlayRunSound(int index)
    {
        AudioSource.PlayOneShot(RunClips[index]);
    }
}
