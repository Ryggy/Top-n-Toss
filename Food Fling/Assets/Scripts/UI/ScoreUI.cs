using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private int totalMoney = 0;
    public TMP_Text moneyText;
    public TMP_Text upgradesMoneyText;
    private void OnEnable()
    {
        DelegatesManager.Instance.ProgressionEventHandler.OnMoneyChanged += UpdateTotalMoney;
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.ProgressionEventHandler.OnMoneyChanged -= UpdateTotalMoney;
    }

    public void UpdateTotalMoney(int money)
    {
        totalMoney += money;
        moneyText.text = $"MONEY : {totalMoney.ToString()}";
        upgradesMoneyText.text = $"MONEY : {totalMoney.ToString()}";
    }
}
