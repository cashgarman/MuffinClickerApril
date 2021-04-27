using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UpgradeType
{
    MuffinsPerClick,
    MuffinPerSecond,
    Cinnabomb,
}

public class UpgradeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Game game;
    public UpgradeType type;
    public int costPerLevel;
    public TMP_Text levelLabel;
    public TMP_Text costLabel;

    public int level;
    private int cost;

    private void Update()
    {
        // Update the level label
        levelLabel.text = level.ToString();

        // Calculate the cost to upgrade to the next level
        cost = (level + 1) * costPerLevel;

        // Update the cost label
        costLabel.text = cost.ToString();

        // Colour the cost label appropriately
        costLabel.color = cost <= game.numberOfMuffins ? Color.green : Color.red;   // Ternary operator
    }

    public void OnUpgradeClicked()
    {
        // Try to purchase the upgrade
        if(game.TryToPurchaseUpgrade(type, cost))
        {
            // Increase the level of the upgrade
            level++;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1.0f, 0.2f);
    }
}
