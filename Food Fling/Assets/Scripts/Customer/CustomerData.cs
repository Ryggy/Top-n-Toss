using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomerData
{
    public int CustomerID  { get; private set; }
    public string CustomerName;
    public List<Ingredient> PreferredIngredients = new List<Ingredient>();
    public List<Ingredient> DislikedIngredients = new List<Ingredient>();
    public float Patience;
    public float Satisfaction;
    public bool IsVIP;
    public bool IsVegetarian;
    private static int _customerID = 0;
    
    public CustomerData()
    {
        CustomerID = ++_customerID;
    }
}
