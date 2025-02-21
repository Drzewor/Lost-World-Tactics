using System;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public static CoverManager Instance {get; private set;}
    private GridSystem<GridObject> gridSystem;
    private List<GridPosition> coverGridPositionList;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private void Awake() 
    {        
        if(Instance != null)
        {
            Debug.LogError($"There is more than one CoverManager {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width,int height, GridSystem<GridObject> gridSystem)
    {
        this.gridSystem = gridSystem;
        coverGridPositionList = new List<GridPosition>();

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;

                if(Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffsetDistance, 
                    Vector3.up, 
                    out RaycastHit hitInfo,
                    raycastOffsetDistance *2,
                    obstaclesLayerMask))
                {
                    GameObject testGameobject = hitInfo.collider.gameObject;
                    if(testGameobject.TryGetComponent<CoverObject>(out CoverObject coverObject))
                    {
                        CreateCover(gridPosition, coverObject);
                    }
                }
            }
        }

        CoverVisualManager.Instance.Setup(coverGridPositionList);
        
    }

    private void CreateCover(GridPosition gridPosition, CoverObject coverObject)
    {
        Cover.CoverType coverType = coverObject.GetCoverType();
        // at Left Gridposition try to add cover from Right side
        if(gridPosition.x -1 >= 0)
        {
            GridPosition leftGridPosition = new GridPosition(gridPosition.x -1, gridPosition.z);
            if(Pathfinding.Instance.IsWalkableGridPosition(leftGridPosition))
            {
                LevelGrid.Instance.AddCoverAtGridPosition(leftGridPosition,coverType, Cover.CoverDirection.Right);
                if(!coverGridPositionList.Contains(leftGridPosition))
                {
                    coverGridPositionList.Add(leftGridPosition);
                }
            }
        }

        // at Right Gridposition try to add cover from Left side
        if(gridPosition.x +1 < gridSystem.GetWidth())
        {
            GridPosition rightGridPosition = new GridPosition(gridPosition.x +1, gridPosition.z);
            if(Pathfinding.Instance.IsWalkableGridPosition(rightGridPosition))
            {
                LevelGrid.Instance.AddCoverAtGridPosition(rightGridPosition,coverType, Cover.CoverDirection.Left);
                if(!coverGridPositionList.Contains(rightGridPosition))
                {
                    coverGridPositionList.Add(rightGridPosition);
                }
            }
        }

        // at Down Gridposition try to add cover from front
        if(gridPosition.z -1 >= 0)
        {
            GridPosition downGridPosition = new GridPosition(gridPosition.x, gridPosition.z -1);
            if(Pathfinding.Instance.IsWalkableGridPosition(downGridPosition))
            {
                LevelGrid.Instance.AddCoverAtGridPosition(downGridPosition,coverType, Cover.CoverDirection.Front);
                if(!coverGridPositionList.Contains(downGridPosition))
                {
                    coverGridPositionList.Add(downGridPosition);
                }
            }
            
        }

        // at Up Gridposition try to add cover from back
        if(gridPosition.z +1 < gridSystem.GetHeight())
        {
            GridPosition upGridPosition = new GridPosition(gridPosition.x, gridPosition.z + 1);
            if(Pathfinding.Instance.IsWalkableGridPosition(upGridPosition))
            {
                LevelGrid.Instance.AddCoverAtGridPosition(upGridPosition,coverType, Cover.CoverDirection.Back);
                if(!coverGridPositionList.Contains(upGridPosition))
                {
                    coverGridPositionList.Add(upGridPosition);
                }
            }
        }
    }

    public void RemoveCover(GridPosition gridPosition)
    {
        //Check for RightCover at Left GridPosition
        if(gridPosition.x -1 >= 0)
        {
            GridPosition leftGridPosition = new GridPosition(gridPosition.x -1, gridPosition.z);
            if(Pathfinding.Instance.IsWalkableGridPosition(leftGridPosition))
            {
                GridObject gridObject = gridSystem.GetGridObject(leftGridPosition);
                foreach (Cover cover in gridObject.GetCoverList())
                {
                    if(cover.GetCoverDirection() == Cover.CoverDirection.Right)
                    {
                        gridObject.RemoveCover(cover);
                        CoverVisualManager.Instance.UpdateCoverVisualObject(leftGridPosition);
                        if(LevelGrid.Instance.GetCoverListAtGridPosition(leftGridPosition).Count == 0)
                        {
                            coverGridPositionList.Remove(leftGridPosition);
                        }
                        break;
                    }
                }
            }
        }

        //Check for LeftCover at Right GridPosition
        if(gridPosition.x +1 < gridSystem.GetWidth())
        {
            GridPosition rightGridPosition = new GridPosition(gridPosition.x +1, gridPosition.z);
            if(Pathfinding.Instance.IsWalkableGridPosition(rightGridPosition))
            {
                GridObject gridObject = gridSystem.GetGridObject(rightGridPosition);
                foreach (Cover cover in gridObject.GetCoverList())
                {
                    if(cover.GetCoverDirection() == Cover.CoverDirection.Left)
                    {
                        gridObject.RemoveCover(cover);
                        CoverVisualManager.Instance.UpdateCoverVisualObject(rightGridPosition);
                        if(LevelGrid.Instance.GetCoverListAtGridPosition(rightGridPosition).Count == 0)
                        {
                            coverGridPositionList.Remove(rightGridPosition);
                        }
                        break;
                    }
                }
            }
        }

        //Check for FrontCover at Back GridPosition
        if(gridPosition.z -1 >= 0)
        {
            GridPosition downGridPosition = new GridPosition(gridPosition.x, gridPosition.z -1);
            if(Pathfinding.Instance.IsWalkableGridPosition(downGridPosition))
            {
                GridObject gridObject = gridSystem.GetGridObject(downGridPosition);
                foreach (Cover cover in gridObject.GetCoverList())
                {
                    if(cover.GetCoverDirection() == Cover.CoverDirection.Front)
                    {
                        gridObject.RemoveCover(cover);
                        CoverVisualManager.Instance.UpdateCoverVisualObject(downGridPosition);
                        if(LevelGrid.Instance.GetCoverListAtGridPosition(downGridPosition).Count == 0)
                        {
                            coverGridPositionList.Remove(downGridPosition);
                        }
                        break;
                    }
                }
            }
        }

        //Check for BackCover at Front GridPosition
        if(gridPosition.z +1 < gridSystem.GetHeight())
        {
            GridPosition upGridPosition = new GridPosition(gridPosition.x, gridPosition.z + 1);
            if(Pathfinding.Instance.IsWalkableGridPosition(upGridPosition))
            {
                GridObject gridObject = gridSystem.GetGridObject(upGridPosition);
                foreach (Cover cover in gridObject.GetCoverList())
                {
                    if(cover.GetCoverDirection() == Cover.CoverDirection.Back)
                    {
                        gridObject.RemoveCover(cover);
                        CoverVisualManager.Instance.UpdateCoverVisualObject(upGridPosition);
                        if(LevelGrid.Instance.GetCoverListAtGridPosition(upGridPosition).Count == 0)
                        {
                            coverGridPositionList.Remove(upGridPosition);
                        }
                        break;
                    }
                }
            }
        }
    }
}
