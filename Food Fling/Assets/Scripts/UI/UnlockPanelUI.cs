using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnlockPanelUI : MonoBehaviour
{
    public GameObject unlockPanel;
    public List<GameObject> ingredientIcons = new List<GameObject>();
    public TMP_Text ingredientNameText;
    private void OnEnable()
    {
        DelegatesManager.Instance.ProgressionEventHandler.OnIngredientUnlocked += ShowUnlockPanel;
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.ProgressionEventHandler.OnIngredientUnlocked += ShowUnlockPanel;
    }

    private void ShowUnlockPanel(string ingredient)
    {
        foreach (var icon in ingredientIcons)
        {
            icon.SetActive(false);
        }
        
        var tempIngredient = OrderManager.Instance.AllIngredients[ingredient];
        ingredientNameText.text = tempIngredient.ingredientName;

        foreach (var icon in ingredientIcons
                     .Where(icon => icon.name == tempIngredient.ingredientName))
        {
            icon.SetActive(true);
        }
        
        unlockPanel.SetActive(true);
    }
}
