using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance {get;private set;}
    public event EventHandler OnAnyUniyMovedGridPosition;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    private GridSystem<GridObject> gridSystem;
    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError($"There is more than one LevelGrid {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem<GridObject>(width,height,cellSize, 
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g,gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start() 
    {
        Pathfinding.Instance.Setup(width,height,cellSize);
        CoverManager.Instance.Setup(width,height,gridSystem);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition,unit);

        OnAnyUniyMovedGridPosition.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool isValidGridPosition(GridPosition gridPosition) => gridSystem.isValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }

    public void AddCoverAtGridPosition(GridPosition gridPosition, Cover.CoverType coverType, Cover.CoverDirection coverDirection)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddCover(coverType,coverDirection);
    }

    public List<Cover> GetCoverListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetCoverList();
    }

    public void RemoveCoverAtGridPosition(GridPosition gridPosition, Cover cover)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveCover(cover);
    }
}
