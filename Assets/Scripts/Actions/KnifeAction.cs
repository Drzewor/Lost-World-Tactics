using System;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAction : BaseAction
{
    public static event EventHandler OnAnyKnifeHit;
    public event EventHandler OnKnifeActionStarted;
    public event EventHandler OnKnifeActionCompleted;

    private enum State{
        SwingingKnifeBeforeHit,
        SwingingKnifeAfterHit
    }
    private int maxKnifeDistance = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update() 
    {
        if(!isActive) return;
        
        stateTimer -=Time.deltaTime;

        switch (state)
        {
            case State.SwingingKnifeBeforeHit:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime*rotateSpeed);
                break;
            case State.SwingingKnifeAfterHit:
                break;
            default:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch(state)
        {
            case State.SwingingKnifeBeforeHit:
                float afterHitStateTime = 0.35f;
                stateTimer = afterHitStateTime;

                targetUnit.Damage(100);
                OnAnyKnifeHit.Invoke(this,EventArgs.Empty);

                state = State.SwingingKnifeAfterHit;
                break;
            case State.SwingingKnifeAfterHit:
                OnKnifeActionCompleted?.Invoke(this,EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Knife";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxKnifeDistance; x <= maxKnifeDistance; x++)
        {
            for (int z = -maxKnifeDistance; z <= maxKnifeDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //grid is empty;
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //same team
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingKnifeBeforeHit;
        float beforeHitStateTime = 0.66f;
        stateTimer = beforeHitStateTime;

        OnKnifeActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetMaxKnifeDistance()
    {
        return maxKnifeDistance;
    }
}
