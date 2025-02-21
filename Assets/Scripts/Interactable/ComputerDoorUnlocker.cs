using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDoorUnlocker : MonoBehaviour, IInteractable
{
    [SerializeField] private bool shouldOpenDoors = false;
    [SerializeField] private List<Door> doorList;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive = false;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);
    }

    void Update()
    {
        if(!isActive) return;

        onInteractionComplete();
        isActive = false;
    }

    public void Interact(Action onInteractionComplete, Unit unit)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        
        foreach(Door door in doorList)
        {
            if(door.GetIsLocked())
            {
                if(shouldOpenDoors)
                {
                    door.UnlockAndOpenDoor();
                }
                else
                {
                    door.UnlockDoor();
                }
            }
        }
    }
}
