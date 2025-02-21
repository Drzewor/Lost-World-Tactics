using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    void Start()
    {
        DestructableCrate.OnAnyDestroy += DestructableCrate_OnAnyDestroy;
    }

    private void DestructableCrate_OnAnyDestroy(object sender, EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        Pathfinding.Instance.SetWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
