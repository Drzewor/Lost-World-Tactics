using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public event EventHandler OnAim;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingunit;
        public bool didHitTarget;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    private State state;
    private int maxShootDistance= 8;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;
    private bool isAiming;

    private void Update() 
    {
        if(!isActive) return;
        
        stateTimer -=Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Aim();
                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
            default:
                break;
        }

        if (stateTimer <=0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch(state)
        {
            case State.Aiming:
                if (stateTimer <=0f)
                {
                    state = State.Shooting;
                    float shootingStateTimel = 0.1f;
                    stateTimer = shootingStateTimel;
                }
                break;
            case State.Shooting:
                if (stateTimer <=0f)
                {
                    state = State.Cooloff;
                    float cooloffStateTimel = 0.5f;
                    stateTimer = cooloffStateTimel;
                }
                break;
            case State.Cooloff:
                if (stateTimer <=0f)
                {
                    ActionComplete();
                }
                break;
        }
    }

    private void Aim()
    {
        if(!isAiming)
        {
            OnAim?.Invoke(this, EventArgs.Empty);
            isAiming = true;
        }
        
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime*rotateSpeed);
    }

    private void Shoot()
    {
        isAiming = false;

        int damageAmount = 50;
        int hitChance = unit.GetAccuracy();
        bool isShootAccurate;

        List<Cover> coverList = LevelGrid.Instance.GetCoverListAtGridPosition(targetUnit.GetGridPosition());
        foreach (Cover cover in coverList)
        {
            if(cover.IsInCover(unit.GetGridPosition(), out Cover.CoverType coverType))
            {
                if(coverType == Cover.CoverType.HalfCover)
                {
                    //reducing chance of hitting target by -25%
                    hitChance -= 25;
                    break;
                }
                if(coverType == Cover.CoverType.FullCover)
                {
                    //reducing chance of hitting target by -50%
                    hitChance -= 50;
                    break;
                }
            }
        }

        int hitRoll = UnityEngine.Random.Range(1,101);

        if(hitRoll > hitChance)
        {
            isShootAccurate = false;
            Debug.Log("Miss!");
        }
        else
        {
            isShootAccurate = true;
            targetUnit.Damage(damageAmount);
            Debug.Log("Hit!");
        }

        OnShoot?.Invoke(this, new OnShootEventArgs {
            targetUnit = targetUnit,
            shootingunit = unit,
            didHitTarget = isShootAccurate
        });
        OnAnyShoot?.Invoke(this, new OnShootEventArgs {
            targetUnit = targetUnit,
            shootingunit = unit,
            didHitTarget = isShootAccurate
        });
    }

    public override string GetActionName()
    {
        return "Shoot";
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance)
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;
                
                if(Physics.Raycast(unitWorldPosition + Vector3.up *unitShoulderHeight,
                    shootDirection,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    //block by obstacles
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



        state = State.Aiming;
        float aimingStateTimel = 1f;
        stateTimer = aimingStateTimel;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootdistance()
    {
        return maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 -targetUnit.GetHealthNormalized()) * 100f)
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
