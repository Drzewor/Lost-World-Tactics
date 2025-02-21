

using UnityEngine;

public class Cover
{
    public enum CoverType {
        HalfCover = 10,
        FullCover = 20
    }

    public enum CoverDirection {
        Front,
        Back,
        Left,
        Right
    }

    private CoverType coverType;
    private CoverDirection coverDirection;
    private GridPosition gridPosition;

    public Cover(
        GridPosition gridPosition, 
        CoverType coverType, 
        CoverDirection coverDirection)
    {
        this.gridPosition = gridPosition;
        this.coverType = coverType;
        this.coverDirection = coverDirection;
    }

    public CoverType GetCoverType()
    {
        return coverType;
    }

    public void SetCoverType(CoverType coverType)
    {
        this.coverType = coverType;
    }

    public CoverDirection GetCoverDirection()
    {
        return coverDirection;
    }

    public void SetCoverDirection(CoverDirection coverDirection)
    {
        this.coverDirection = coverDirection;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsInCover(GridPosition shooterGridPosition, out CoverType coverType)
    {
        coverType = this.coverType;
        //Debug.Log(coverDirection);
        switch (coverDirection)
        {
            case CoverDirection.Right:
                //Debug.Log(gridPosition);
                //Debug.Log(shooterGridPosition);
                //Debug.Log("cover x " + gridPosition.x);
                //Debug.Log("shooter x " + shooterGridPosition.x);
                if(gridPosition.x < shooterGridPosition.x)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CoverDirection.Left:
                if(gridPosition.x > shooterGridPosition.x)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CoverDirection.Front:
                if(gridPosition.z < shooterGridPosition.z)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CoverDirection.Back:
                if(gridPosition.z > shooterGridPosition.z)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}
