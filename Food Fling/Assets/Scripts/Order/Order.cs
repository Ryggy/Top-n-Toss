using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Order
{
    public OrderData Data { get; private set; }
    private MultiplierManager MultiplierManager;
    public Order(CustomerData customerData, MultiplierManager multiplierManager, int numOfIngredients)
    {
        Data = new OrderData(customerData);
        MultiplierManager = multiplierManager;
        Data.RequiredIngredients = GenerateIngredients(numOfIngredients);
        UpdateStatus(OrderStatus.Pending);
    }
    
    public void UpdateOrder(float deltaTime)
    {
        if (!Data.IsTimed) return;
        Data.OrderTimer += deltaTime;
        // update patience of customer
        DelegatesManager.Instance.TriggerOnCustomerPatienceChange(Data.CustomerData);
        if (Data.OrderTimer < Data.TimeLimit) return;
        // if we are here, the order is timed and we are over the time limit
        OrderManager.Instance.FailOrder(Data.OrderID);
        Data.IsTimed = false;
    }
    
    /// <summary>
    /// Generates a list of ingredients based on the unlocked ingredients and customer preferences
    /// </summary>
    /// <param name="numOfIngredients"></param>
    public List<Ingredient> GenerateIngredients(int numOfIngredients = 2)
    {
        var ingredients = new List<Ingredient>();
        var allIngredients = OrderManager.Instance.AllIngredients;

        if (allIngredients.Count < 1)
        {
            Debug.LogError("No ingredients found in allIngredients Dictionary");
            return null;
        }

        // Filter unlocked ingredients
        var tempIngredientList = allIngredients
            .Where(ingredient => ingredient.Value.isUnlocked)
            .Select(ingredient => ingredient.Value).ToList();

        if (tempIngredientList.Count < 1)
        {
            Debug.LogError("No ingredients unlocked");
            return null;
        }

        // Filter vegetarian ingredients if the customer is vegetarian
        if (Data.CustomerData.IsVegetarian)
        {
            tempIngredientList = tempIngredientList.Where(ingredient => ingredient.isVegetarian).ToList();
        }

        // Ensure at least one sauce and remove all sauces from the list
        var sauces = tempIngredientList.Where(ingredient => ingredient.IngredientType == IngredientType.Sauce).ToList();
        if (sauces.Count > 0)
        {
            var selectedSauce = sauces[Random.Range(0, sauces.Count)];
            ingredients.Add(selectedSauce);
            tempIngredientList.RemoveAll(ingredient => ingredient.IngredientType == IngredientType.Sauce);
        }
        else
        {
            Debug.LogWarning("No sauce ingredients available.");
        }

        // Ensure at least one cheese and remove all cheeses from the list
        var cheeses = tempIngredientList.Where(ingredient => ingredient.IngredientType == IngredientType.Cheese).ToList();
        if (cheeses.Count > 0)
        {
            var selectedCheese = cheeses[Random.Range(0, cheeses.Count)];
            ingredients.Add(selectedCheese);
            tempIngredientList.RemoveAll(ingredient => ingredient.IngredientType == IngredientType.Cheese);
        }
        else
        {
            Debug.LogWarning("No cheese ingredients available.");
        }

        // Add remaining ingredients randomly to meet the desired count
        for (var i = ingredients.Count; i < numOfIngredients; i++)
        {
            if (tempIngredientList.Count == 0)
            {
                Debug.LogWarning("Not enough ingredients to fulfill the order.");
                break;
            }

            var ingredient = tempIngredientList[Random.Range(0, tempIngredientList.Count)];
            ingredients.Add(ingredient);
            tempIngredientList.Remove(ingredient);
        }

        return ingredients;
    }
    
    /// <summary>
    /// Updates the order’s status (e.g., from Pending to InProgress or Completed).
    /// </summary>
    /// <param name="newStatus"></param>

    public void UpdateStatus(OrderStatus newStatus)
    {
        Data.Status = newStatus;
        switch (newStatus)
        {
            case OrderStatus.Pending:

                break;
            case OrderStatus.InProgress:
                DelegatesManager.Instance.TriggerOnOrderGenerated(Data);
                break;
            case OrderStatus.Completed:
                DelegatesManager.Instance.TriggerOnOrderCompleted(Data, true);
                break;
            case OrderStatus.Failed:
                DelegatesManager.Instance.TriggerOnOrderCompleted(Data, false);
                break;
        }
    }
    
    /// <summary>
    /// Compares the player’s constructed pizza with the RequiredIngredients and
    /// returns an accuracy score based on how closely they match.
    /// </summary>
    /// <param name="pizza"></param>
    public void EvaluatePizza(Pizza pizza)
    {
        var accuracy = 0f;
        var matchingIngredients = 0;
        
        foreach (var ingredient in pizza.Data.IngredientsOnPizza)
        {
            if (Data.RequiredIngredients.Any(reqIngredient => reqIngredient.Equals(ingredient)))
            {
                matchingIngredients++;
            }
        }

        accuracy = (float)matchingIngredients / Data.RequiredIngredients.Count;
        Debug.Log($"Pizza Evaluation: {matchingIngredients}/{Data.RequiredIngredients.Count} ingredients match.");
        Debug.Log($"Accuracy Score: {accuracy * 100}%");
        
        if (Data.IsTimed && IsOrderTimedOut(Data.OrderTimer))
        {
            accuracy -= 50f;
        }

        Data.AccuracyScore = accuracy;
    }

    /// <summary>
    /// Checks if the time for a timed order has expired.
    /// Returns true if the order's time limit has been exceeded.
    /// </summary>
    /// <param name="elapsedTime"></param>
    /// <returns></returns>
    public bool IsOrderTimedOut(float elapsedTime)
    {
        return Data.IsTimed && elapsedTime >= Data.TimeLimit;
    }

    /// <summary>
    /// Calculates the reward based on the customer’s patience,
    /// satisfaction, whether they are a VIP and pizza accuracy
    /// </summary>
    /// <returns></returns>
    public int CalculateReward()
    {
        var customer = Data.CustomerData;
        var totalCost = Data.RequiredIngredients.Sum(ingredient => ingredient.cost);
        return (int)((customer.Patience + customer.Satisfaction + (customer.IsVIP ? 100 : 0) + totalCost) * Data.AccuracyScore * MultiplierManager.multTotal);
    }
}
