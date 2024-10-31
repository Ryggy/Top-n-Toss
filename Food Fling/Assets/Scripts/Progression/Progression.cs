using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class Progression
{
    // singleton
    private static Progression instance = null;
    private static readonly object padlock = new object();
    Progression()
    {
    }
    public static Progression Instance
    {
        get
        {
            lock (padlock)
            {
                return instance ??= new Progression();
            }
        }
    }
    
    public ProgressionData Data { get; private set; } = new ProgressionData();

    public void GainMoney(int moneyIncrement)
    {
        Data.TotalMoney += moneyIncrement;
        DelegatesManager.Instance.TriggerOnMoneyUpdated(moneyIncrement);
        if (moneyIncrement > 0)
        {
            if (IsProgressionThresholdMet(Data.MoneyThreshold, Data.TotalMoney))
            {
                Data.MoneyThreshold += 500;
                LevelUp();
            }
        }
    }


    public void LevelUp()
    {
        Data.CurrentLevel++;
        DelegatesManager.Instance.TriggerOnLevelUp(Data.CurrentLevel);

        if (CheckForUpgradeUnlock())
        {
            UnlockUpgrade(Data.CurrentLevel);
        }

        if (CheckForIngredientUnlock())
        {
            UnlockIngredient(Data.CurrentLevel);
        }
    }

    public void UnlockIngredient(int level)
    {
        var ingredient = level switch
        {
            2 => "BBQSauce",
            4 => "Pepperoni",
            6 => "Pineapple",
            _ => ""
        };

        if (ingredient == "") return;
        
        OrderManager.Instance.AllIngredients[ingredient].isUnlocked = true;
        DelegatesManager.Instance.TriggerOnIngredientUnlocked(ingredient);
    }

    public void UnlockUpgrade(int level)
    {
        string upgrade = "???";
        DelegatesManager.Instance.TriggerOnUpgradeUnlocked(upgrade);
    }

    private bool IsProgressionThresholdMet(int xpThreshold, int currentXp)
    {
        return currentXp >= xpThreshold;
    }

    private bool CheckForUpgradeUnlock()
    {
        var unlockUpgrade = true;
        // TODO: add some logic for determining when upgrades unlock on what level
        return unlockUpgrade;
    }

    private bool CheckForIngredientUnlock()
    {
        var unlockIngredient = true;
        // TODO: add some logic for determining when ingredients unlock on what level
        return unlockIngredient;
    }
}
