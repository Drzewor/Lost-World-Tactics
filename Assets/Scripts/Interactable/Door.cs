using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] protected bool isLocked = false;
    [SerializeField] private Transform lockMessagePrefab;
    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive = false;
    private float timer;

    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        if(!isActive)
        {
            return;
        }

        if(isLocked)
        {
            onInteractionComplete();
        }

        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            onInteractionComplete();
            isActive = false;
        }
    }

    protected virtual void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);

        if(isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public virtual void Interact(Action onInteractionComplete,Unit unit)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        if(isLocked) 
        {
            Instantiate(lockMessagePrefab,unit.transform.position, Quaternion.identity);
            return;
        }

        if(isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    protected virtual void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen",isOpen);
        Pathfinding.Instance.SetWalkableGridPosition(gridPosition,true);
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen",isOpen);
        Pathfinding.Instance.SetWalkableGridPosition(gridPosition,false);
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }

    public void UnlockAndOpenDoor()
    {
        UnlockDoor();
        OpenDoor();
    }

    public bool GetIsLocked()
    {
        return isLocked;
    }
}
