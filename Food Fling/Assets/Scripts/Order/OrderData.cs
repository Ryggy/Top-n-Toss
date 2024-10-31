using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrderData
{
    public int OrderID { get; private set; }

    public CustomerData CustomerData;
    public List<Ingredient> RequiredIngredients = new List<Ingredient>();
    public bool IsTimed = false;
    public float OrderTimer = 0f;
    public float TimeLimit = 60f;
    public OrderStatus Status;
    public float AccuracyScore = 0f;

    private static int _orderCounter = 0;

    public OrderData(CustomerData customerData)
    {
        CustomerData = customerData;
        OrderID = ++_orderCounter;
    }
}
