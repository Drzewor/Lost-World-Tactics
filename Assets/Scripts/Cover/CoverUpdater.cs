using System;
using UnityEngine;

public class CoverUpdater : MonoBehaviour
{

    void Start()
    {
        DestructableCrate.OnAnyDestroy += DestructableCrate_OnAnyDestroy;
    }

    private void DestructableCrate_OnAnyDestroy(object sender, EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        CoverManager.Instance.RemoveCover(destructableCrate.GetGridPosition());
    }
}
