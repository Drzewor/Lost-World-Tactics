using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Update() 
    {
        if(!isActive) return;
        
        float spinnAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0,spinnAddAmount,0);
        
        totalSpinAmount += spinnAddAmount;

        if(totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0;
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}
