using System;
using UnityEngine;

public class ProgressionEventHandler :IProgressionEventHandler
{
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnLevelUp;
    public event Action<string> OnUpgradeUnlocked;
    public event Action<string> OnIngredientUnlocked;
    public void MoneyUpdated(int moneyIncrement)
    {
        OnMoneyChanged?.Invoke(moneyIncrement);
    }
    
    public void LevelUp(int newLevel)
    {
        OnLevelUp?.Invoke(newLevel);
    }

    public void UpgradeUnlocked(string upgrade)
    {
        OnUpgradeUnlocked?.Invoke(upgrade);
    }

    public void IngredientUnlocked(string ingredient)
    {
        OnIngredientUnlocked?.Invoke(ingredient);
    }
}

public interface IProgressionEventHandler
{
    event Action<int> OnMoneyChanged;
    event Action<int> OnLevelUp;
    event Action<string> OnUpgradeUnlocked;
    event Action<string> OnIngredientUnlocked;
    
    void MoneyUpdated(int moneyIncrement);
    void LevelUp(int newLevel);
    void UpgradeUnlocked(string upgrade);
    void IngredientUnlocked(string ingredient);
}
