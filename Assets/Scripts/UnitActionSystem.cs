using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnActionStarted;
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask UnitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError($"There is more than one UnitActionSystem {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update() 
    {
        if(isBusy) return;
        
        if(!TurnSystem.Instance.IsPlayerTurn()) return;

        if(EventSystem.current.IsPointerOverGameObject()) return;
        
        if(TryHandleUnitSelection()) return;

        if(selectedUnit == null) return;
         
        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, UnitLayerMask))
            {
                if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit == selectedUnit || unit.IsEnemy()) 
                    {
                        return false;
                    }
                    else
                    {
                        SetSelectedUnit(unit);
                        return true;
                    }

                }
            }
        }
        return false;
    }
    
    private void HandleSelectedAction()
    {
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
 
            if(selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition,ClearBusy);

                OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    internal BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
