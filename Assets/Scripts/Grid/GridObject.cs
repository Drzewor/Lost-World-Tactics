using System.Collections.Generic;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractable interactable;
    private List<Cover> coverList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
        coverList = new List<Cover>();
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach(Unit unit in unitList)
        {
            unitString += unit + "\n";
        }
        
        return $"{gridPosition.ToString()}\n{unitString}";
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }

    public IInteractable GetInteractable()
    {
        return interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        this.interactable = interactable;
    }

    public void AddCover(Cover.CoverType coverType, Cover.CoverDirection coverDirection)
    {
        Cover cover = new Cover(gridPosition, coverType, coverDirection);
        coverList.Add(cover);
    }

    public void RemoveCover(Cover cover)
    {
        coverList.Remove(cover);
    }

    public List<Cover> GetCoverList()
    {
        return coverList;
    }
}
