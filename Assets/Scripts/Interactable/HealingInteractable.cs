using System;
using UnityEngine;

public class HealingInteractable : MonoBehaviour ,IInteractable
{
    [SerializeField] int HealingAmount;
    Action onInteractionComplete;
    GridPosition gridPosition;
    bool isActive = false;

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);
    }

    void Update()
    {
        if(isActive)
        {
            onInteractionComplete();
            isActive = false;
        }
    }

    public void Interact(Action onInteractionComplete,Unit unit)
    {
        if(unit.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.Heal(HealingAmount);
        }
        isActive = true;

        this.onInteractionComplete = onInteractionComplete;
    }
}
