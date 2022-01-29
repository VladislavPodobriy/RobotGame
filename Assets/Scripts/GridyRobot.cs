using UnityEngine;

public class GridyRobot : InteractableObject
{
    private Animator _anim;
    public Transform Pivot;
    public ItemObject Chair;
    
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
}
