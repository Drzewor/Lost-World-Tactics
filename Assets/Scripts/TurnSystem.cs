using System;
using UnityEngine;
public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance {get; private set;}
    public event EventHandler onTurnChange;
    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError($"There is more than one TurnSystem {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        if(isPlayerTurn)
        {
            turnNumber++;
        }
        
        onTurnChange.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
