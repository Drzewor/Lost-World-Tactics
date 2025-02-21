using System.Collections.Generic;
using UnityEngine;

public class CoverVisual : MonoBehaviour
{
    [SerializeField] GameObject leftCoverVisualGameObject;
    [SerializeField] GameObject rightCoverVisualGameObject;
    [SerializeField] GameObject frontCoverVisualGameObject;
    [SerializeField] GameObject backCoverVisualGameObject;
    [SerializeField] Transform fullCoverModelPrefab;
    [SerializeField] Transform halfCoverModelPrefab;
    GridPosition gridPosition;

    public void Setup(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
        Setup();
    }

    public void Setup()
    {
        leftCoverVisualGameObject.SetActive(false);
        rightCoverVisualGameObject.SetActive(false);
        frontCoverVisualGameObject.SetActive(false);
        backCoverVisualGameObject.SetActive(false);

        List<Cover> coverList= LevelGrid.Instance.GetCoverListAtGridPosition(gridPosition);

        foreach(Cover cover in coverList)
        {
            Transform coverTransformPrefab = null;

            switch (cover.GetCoverType())
            {
                case Cover.CoverType.HalfCover:
                    coverTransformPrefab = halfCoverModelPrefab;
                    break;
                case Cover.CoverType.FullCover:
                    coverTransformPrefab = fullCoverModelPrefab;
                    break;
            }

            switch (cover.GetCoverDirection())
            {
                case Cover.CoverDirection.Right:
                    rightCoverVisualGameObject.SetActive(true);
                    Instantiate(coverTransformPrefab,rightCoverVisualGameObject.transform);
                    break;
                case Cover.CoverDirection.Left:
                    leftCoverVisualGameObject.SetActive(true);
                    Instantiate(coverTransformPrefab,leftCoverVisualGameObject.transform);
                    break;
                case Cover.CoverDirection.Front:
                    frontCoverVisualGameObject.SetActive(true);
                    Instantiate(coverTransformPrefab,frontCoverVisualGameObject.transform);
                    break;
                case Cover.CoverDirection.Back:
                    backCoverVisualGameObject.SetActive(true);
                    Instantiate(coverTransformPrefab,backCoverVisualGameObject.transform);
                    break;
            }
        }
    }

    public void UpdateVisual()
    {
        List<GameObject> CoverVisualsGameobjectList = new List<GameObject>{
            rightCoverVisualGameObject,
            leftCoverVisualGameObject,
            frontCoverVisualGameObject,
            backCoverVisualGameObject
        };
            
        foreach(GameObject coverVisual in CoverVisualsGameobjectList)
        {
            foreach(Transform child in coverVisual.transform)
            {
                Destroy(child.gameObject);
            }
        }

        List<Cover> coverList= LevelGrid.Instance.GetCoverListAtGridPosition(gridPosition);

        if(coverList.Count != 0)
        {
            Setup();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
