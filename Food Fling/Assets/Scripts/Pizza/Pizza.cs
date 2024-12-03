using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Pizza : MonoBehaviour
{
    public PizzaData Data { get; private set; } = new PizzaData();
    
    public void AddIngredient(string ingredient)
    {
        if (Data.IngredientsOnPizza.Count >= Data.MaxIngredients) return;
        var tempIngredient = OrderManager.Instance.AllIngredients[ingredient];
        Debug.Log($"Added ingredient: {ingredient}");

        foreach(Transform child in transform)
        {
            if(child != null && child.name == ingredient)
            {
                child.gameObject.SetActive(true);
            }
        }

        Data.IngredientsOnPizza.Add(tempIngredient);
    }
    
    public void RemoveIngredient(string ingredient)
    {
        var tempIngredient = OrderManager.Instance.AllIngredients[ingredient];
        Debug.Log($"Removed ingredient: {ingredient}");
        Data.IngredientsOnPizza.Remove(tempIngredient);
    }
    
    public void ResetPizza()
    {
        Data.IngredientsOnPizza.Clear();
        Debug.Log("Pizza reset!");

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    
    public void SubmitPizza(Customer customer = null)
    {
        if (customer != null && customer.CustomerOrder.Count > 0)
        {
            Order tempOrder = customer.CustomerOrder[0];
            DelegatesManager.Instance.OrderEventHandler.OrderSubmitted(tempOrder.Data, this);
            tempOrder.EvaluatePizza(this);
            customer.UpdateSatisfaction(this);
            
            var reward = tempOrder.CalculateReward();
            OrderManager.Instance.CompleteOrder(tempOrder.Data.OrderID);
            Progression.Instance.GainMoney(reward);
        }
        ResetPizza();
    }
}
