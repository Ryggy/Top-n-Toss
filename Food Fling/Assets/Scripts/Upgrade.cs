using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public GameObject upgradeButton; // Direct reference to the UI button
    public TMP_Text costText; // Direct reference to the cost display text
    public GameObject ingredientContainer;
    [HideInInspector]
    public Ingredient ingredient;

    private void Start()
    {
        ingredient = OrderManager.Instance.AllIngredients[upgradeButton.name];
        costText.text = ingredient.cost.ToString();
    }

    public void Purchase()
    {
        if (Progression.Instance.Data.TotalMoney >= ingredient.cost)
        {
            // Deduct money and update costs
            Progression.Instance.GainMoney(-ingredient.cost);
            ingredient.upgradeCost += 50;
            ingredient.cost += 5;

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money to purchase this upgrade.");
        }
    }

    public void UpdateUI()
    {
        if (costText != null)
        {
            costText.text = $"{ingredient.cost}";
        }
    }
    
    public void Unlock()
    {
        ingredient.Unlock();
        if (upgradeButton != null)
        {
            upgradeButton.GetComponent<Button>().enabled = true;
            if (upgradeButton.transform.childCount > 2)
            {
                upgradeButton.transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        if (ingredientContainer != null)
        {
            ingredientContainer.SetActive(true);
        }
    }
}
