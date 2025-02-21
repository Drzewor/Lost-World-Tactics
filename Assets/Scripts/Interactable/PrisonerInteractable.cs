using System;
using UnityEngine;

public class PrisonerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform unitPrefab;
    [SerializeField] private float timeToSpawn = 1.5f;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private Animator animator;
    private float timer = 0;
    private bool isActive = false;


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(!isActive) return;

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Vector3 position = LevelGrid.Instance.GetWorldPosition(gridPosition);
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,null);
            Pathfinding.Instance.SetWalkableGridPosition(gridPosition,true);
            Instantiate(unitPrefab,position,transform.rotation);
            onInteractionComplete();
            Destroy(gameObject);
        }

        
    }

    public void Interact(Action onInteractionComplete, Unit unit)
    {
        this.onInteractionComplete = onInteractionComplete;

        animator.SetTrigger("Standup");
        
        timer = timeToSpawn;

        isActive = true;
    }
}
