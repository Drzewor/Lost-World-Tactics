using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnNumberText;
    [SerializeField] Button endTurnButton;
    [SerializeField] GameObject enemyTunrVisualGameObject;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() => 
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.onTurnChange += TurnSystem_OnturnChange;

        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        UpdateTurnText();
    }

    private void TurnSystem_OnturnChange(object sender, EventArgs e)
    {
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = $"TURN {TurnSystem.Instance.GetTurnNumber()}";
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTunrVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
