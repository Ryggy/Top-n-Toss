using System.Collections;
using System.Collections.Generic;
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
    
    public int currentLevel = 1;
    private int _xp = 0;
    private int _xpThreshold = 100;
    
    public List<string> unlockedUpgrades = new List<string>();

    public int totalDeliveriesCompleted = 0;
    public int starsEarned = 0;

    public void GainXp(int xpAmount)
    {
        _xp += xpAmount;

        if (IsProgressionThresholdMet(_xpThreshold, _xp))
        {
            _xpThreshold += 100;
            LevelUp();
        }
        
    }

    public void LevelUp()
    {
        currentLevel++;

        if(CheckForUpgradeUnlock())
        {
            UnlockUpgrade();
        }

        if (CheckForIngredientUnlock())
        {
            UnlockIngredient();
        }
    }

    public void UnlockIngredient()
    {
        OrderManager.Instance.AllIngredients["Pineapple"].isUnlocked = true;
        Debug.Log("A NEW INGREDIENT IS UNLOCKED! : Pineapple");
    }

    public void UnlockUpgrade()
    {
        Debug.Log("A NEW UPGRADE IS UNLOCKED! : ???");
    }

    private bool IsProgressionThresholdMet(int xpThreshold, int currentXp)
    {
        return currentXp >= xpThreshold;
    }

    private void UpdateProgressionUI()
    {
        
    }

    private void CalculateStars(int score)
    {
        
    }

    private void TrackDeliveryCompletion()
    {
        
    }

    private bool CheckForUpgradeUnlock()
    {
        var unlockUpgrade = currentLevel == 2;
        
        // TODO: add some logic for determining when upgrades unlock on what level
        
        return unlockUpgrade;
    }

    private bool CheckForIngredientUnlock()
    {
        var unlockIngredient = currentLevel == 2;

        // TODO: add some logic for determining when ingredients unlock on what level
        
        return unlockIngredient;
    }
}
