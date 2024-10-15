using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Pizza : MonoBehaviour
{
    // A list that stores all the ingredients placed on the pizza base.
    public List<Ingredient> IngredientsOnPizza = new List<Ingredient>();
    // The maximum number of ingredients allowed on the pizza, depending on the recipe complexity.
    public int maxIngredients = 5;
    // A flag that determines if the pizza construction has been finalised.
    public bool isPizzaCorrect = false;
    // Represents the type of pizza base (e.g., thin crust, thick crust) chosen for the order.
    // they may have different cook times?
    //public PizzaBase PizzaBase;

    public void AddIngredient(string ingredient)
    {
        if (IngredientsOnPizza.Count >= maxIngredients) return;
        var tempIngredient = OrderManager.Instance.AllIngredients[ingredient];
        Debug.Log($"Added ingredient: {ingredient}");
        IngredientsOnPizza.Add(tempIngredient);
    }

    public void RemoveIngredient(string ingredient)
    {
        var tempIngredient = OrderManager.Instance.AllIngredients[ingredient];
        Debug.Log($"Removed ingredient: {ingredient}");
        IngredientsOnPizza.Remove(tempIngredient);
    }
    public bool CheckIngredientCompatability(string ingredient)
    {
        var tempIngredient = OrderManager.Instance.AllIngredients[ingredient];
        Debug.Log($"Checking compatibility for ingredient: {ingredient}");
        return OrderManager.Instance.CurrentOrder.RequiredIngredients.Contains(tempIngredient);
    }

    public bool IsPizzaCorrect()
    {
        var requiredIngredientIDs = OrderManager.Instance.CurrentOrder.RequiredIngredients.Select(i => i.ingredientID);
        var pizzaIngredientIDs = IngredientsOnPizza.Select(i => i.ingredientID);
        isPizzaCorrect = requiredIngredientIDs.All(pizzaIngredientIDs.Contains);
        return isPizzaCorrect;
    }

    public void ResetPizza()
    {
        IngredientsOnPizza.Clear();
        isPizzaCorrect = false;
        Debug.Log("Pizza reset!");
    }

    public void SubmitPizza()
    {
        IsPizzaCorrect();
        
        // this would be where the pizza in placed on a slingshot and players flings it
        
        // TODO: pass in the customer the pizza actually hits and check active orders, currently hard codes we hit the right customer
        Order tempOrder = OrderManager.Instance.CurrentOrder; // this would be the order of the customer we hit
        tempOrder.EvaluatePizza(this);
        tempOrder.customerDetails.UpdateSatisfaction(this);
        var reward = tempOrder.customerDetails.CalculateReward(tempOrder.orderID);
        OrderManager.Instance.CompleteOrder(tempOrder.orderID);
        // add reward
        Progression.Instance.GainXp(reward);
    }
}
