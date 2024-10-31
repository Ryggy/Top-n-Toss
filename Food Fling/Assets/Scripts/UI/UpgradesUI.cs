using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    public List<Upgrade> upgrades = new List<Upgrade>();

    private void Start()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.ingredient.isUnlocked)
            {
                upgrade.Unlock();
            }
        }
    }

    public void OnEnable()
    {
        DelegatesManager.Instance.ProgressionEventHandler.OnIngredientUnlocked += UnlockIngredient;
    }

    public void OnDisable()
    {
        DelegatesManager.Instance.ProgressionEventHandler.OnIngredientUnlocked -= UnlockIngredient;
    }

    private void UnlockIngredient(string ingredientName)
    {
        var upgrade = upgrades.Find(u => u.ingredient.ingredientName == ingredientName);
        if (upgrade != null)
        {
            upgrade.Unlock();
        }
    }
}
