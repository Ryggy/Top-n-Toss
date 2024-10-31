using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public int totalMoney = 0;
    public TMP_Text moneyText;


    public void UpdateTotalMoney(int money)
    {
        totalMoney += money;
        moneyText.text = totalMoney.ToString();
    }
}
