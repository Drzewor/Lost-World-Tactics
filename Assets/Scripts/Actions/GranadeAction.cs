using System;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAction : BaseAction
{
    [SerializeField] private Transform granadeProjectilePrefab;

    private int maxThrowDistance = 6;
    private void Update() 
    {
        if(!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Granade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform granadeProjectileTransform = 
            Instantiate(granadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GranadeProjectile granadeProjectile = granadeProjectileTransform.GetComponent<GranadeProjectile>();
        granadeProjectile.Setup(gridPosition, OnGranadeBehaviorComplete);
        ActionStart(onActionComplete);
    }

    private void OnGranadeBehaviorComplete()
    {
        ActionComplete();
    }
}
