using System;
using System.Collections.Generic;
using UnityEngine;

public class CoverVisualManager : MonoBehaviour
{
    public static CoverVisualManager Instance {get; private set;}
    [SerializeField] private GameObject gridCoverVisualPrefab;
    private List<GridPosition> coverGridPositionList;
    private List<CoverVisual> coverVisualList;

    private void Awake() 
    {        
        if(Instance != null)
        {
            Debug.LogError($"There is more than one CoverVisualManager {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UpdateVisuals();

        LevelGrid.Instance.OnAnyUniyMovedGridPosition += LevelGrid_OnAnyUniyMovedGridPosition;
    }

    private void LevelGrid_OnAnyUniyMovedGridPosition(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    public void Setup(List<GridPosition> coverGridPositionList)
    {
        this.coverGridPositionList = coverGridPositionList;
        coverVisualList = new List<CoverVisual>();
        CreateVisuals();
    }

    private void CreateVisuals()
    {
        foreach (GridPosition gridPosition in coverGridPositionList)
        {
            Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            GameObject coverVisualGameObject = Instantiate(gridCoverVisualPrefab, worldPosition, Quaternion.identity);
            if(coverVisualGameObject.TryGetComponent<CoverVisual>(out CoverVisual coverVisual))
            {
                coverVisualList.Add(coverVisual);
                coverVisual.Setup(gridPosition);
            }
        }
    }

    public void UpdateCoverVisualObject(GridPosition gridPosition)
    {
        foreach (CoverVisual coverVisual in coverVisualList)
        {
            if(coverVisual.GetGridPosition() == gridPosition)
            {
                coverVisual.UpdateVisual();

                if(LevelGrid.Instance.GetCoverListAtGridPosition(gridPosition).Count == 0)
                {
                    coverGridPositionList.Remove(gridPosition);
                    coverVisualList.Remove(coverVisual);
                    break;
                }
            }
        }
    }

    public void UpdateVisuals()
    {
        foreach (CoverVisual coverVisual in coverVisualList)
        {
            coverVisual.gameObject.SetActive(false);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        if(!(selectedAction is ShootAction) && !(selectedAction is MoveAction)) return;

        ShowCoverVisualList(selectedAction.GetValidActionGridPositionList());
    }

    private void ShowCoverVisualList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            foreach (CoverVisual coverVisual in coverVisualList)
            {
                if(coverVisual.GetGridPosition() == gridPosition)
                {
                    coverVisual.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
