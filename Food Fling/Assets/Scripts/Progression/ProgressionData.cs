using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionData
{
    public int CurrentLevel { get; set; } = 1;
    public int TotalMoney { get; set; } = 0;
    public int MoneyThreshold { get; set; } = 500;
    public List<string> UnlockedIngredients { get; set; } = new List<string>();
    public int TotalDeliveriesCompleted { get; set; } = 0;
}