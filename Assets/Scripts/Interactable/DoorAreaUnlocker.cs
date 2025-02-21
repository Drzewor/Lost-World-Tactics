using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaUnlocker : Door
{
    [SerializeField] private GameObject areaCurtain;
    [SerializeField] private List<GameObject> ListOfObjectsToActivate;
    private bool wasActivated = false;

    protected override void Start()
    {
        base.Start();

        foreach (GameObject gameObject in ListOfObjectsToActivate)
        {
            gameObject.SetActive(false);
        }
    }

    public override void Interact(Action onInteractionComplete, Unit unit)
    {
        base.Interact(onInteractionComplete, unit);

        if(!wasActivated && !isLocked)
        {
            foreach (GameObject gameObject in ListOfObjectsToActivate)
            {
                gameObject.SetActive(true);
            }
            areaCurtain.SetActive(false);
            wasActivated = true;
        }
    }

    protected override void OpenDoor()
    {
        base.OpenDoor();
        if(!wasActivated && !isLocked)
        {
            foreach (GameObject gameObject in ListOfObjectsToActivate)
            {
                gameObject.SetActive(true);
            }
            areaCurtain.SetActive(false);
            wasActivated = true;
        }
    }
}
