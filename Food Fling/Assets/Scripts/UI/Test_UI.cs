using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test_UI : MonoBehaviour
{
    public Pizza pizza;
    public Customer customer1;
    public Customer customer2;
    public Button[] buttons;
    public TMP_Text scoreText;
    private int totalScore = 0;
    private void Start()
    {
        buttons[0].onClick.AddListener(delegate{pizza.SubmitPizza(customer1);});
        buttons[1].onClick.AddListener(delegate{pizza.SubmitPizza(customer2);});
    }

    public void UpdateScore(int score)
    {
        totalScore += score;
        scoreText.text = "Score : " + totalScore.ToString();
    }
}
