using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour, IInteractable
{
    [SerializeField] private int sceneIndex;
    private GridPosition gridPosition;
    private Action onInteractionComplete;
    private bool isActive;

    protected virtual void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);
    }

    void Update()
    {
        if(!isActive)
        {
            return;
        }

        onInteractionComplete();
    }

    public void Interact(Action onInteractionComplete, Unit unit)
    {
        this.onInteractionComplete = onInteractionComplete;
        SceneManager.LoadScene(sceneIndex);
    }
}
