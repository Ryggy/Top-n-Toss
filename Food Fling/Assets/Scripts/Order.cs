using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Order
{
    private static int _orderCounter = 0;
    //A unique identifier for each order instance.
    public int orderID { get; private set; } 
    // Holds the customer information (e.g., preferences, likes, and dislikes) relevant to this order.
    public Customer customerDetails;
    // A list of ingredients the player must add to the pizza to fulfil the order.
    public List<Ingredient> RequiredIngredients;
    // Indicates if the order has a time limit for completion.
    public bool isTimed = false;
    // Tracks the time remaining to complete the order.
    // Affects the reward based on how quickly the player fulfils the order.
    public float orderTimer = 0f;
    // If IsTimed is true, this value represents the time allowed to complete the order.
    public float timeLimit = 60f;
    // Tracks the order’s current state (e.g., Pending, InProgress, Completed).
    public OrderStatus status;
    // Stores the accuracy of how well the player’s pizza matches the customer’s order.
    public float accuracyScore = 0f;

    public Order(Customer customer, int numOfIngredients)
    {
        customerDetails = customer;
        
        // generate ingredients based on number of ingredients
        RequiredIngredients = GenerateIngredients(numOfIngredients);
        
        // TODO: implement methods to set isTimer and timeLimit based on level config
        
        // set order status to pending
        UpdateStatus(OrderStatus.Pending);
        // create a unique id
        orderID = ++_orderCounter;  // Increment the counter and assign the new value
    }

    public void Update(float deltaTime)
    {
        if(isTimed && orderTimer <= timeLimit)
        {
            orderTimer += deltaTime;
            
            OrderManager.Instance.FailOrder(orderID);
            
            isTimed = false;
        }
    }
    
    /// <summary>
    /// Generates a new order based on the customer's preferences, filling the RequiredIngredients list
    /// </summary>
    /// <param name="numOfIngredients"></param>
    public List<Ingredient> GenerateIngredients(int numOfIngredients = 2)
    {
        var ingredients = new List<Ingredient>();
        
        // get all the unlocked ingredients
        var allIngredients = OrderManager.Instance.AllIngredients;

        if (allIngredients.Count < 1)
        {
            Debug.LogError("No ingredients found in allIngredients Dictionary");
            return null;
        }
        
        // filter based on if ingredient is unlocked
        var unlockedIngredients = allIngredients
            .Where(ingredient =>  ingredient.Value.isUnlocked)
            .Select(ingredient => ingredient.Value).ToList();
        
        if (unlockedIngredients!.Count < 1)
        {
            Debug.LogError("No ingredients unlocked");
            return null;
        }
        
        // filter based on if ingredient is vegetarian
        if (customerDetails.isVegetarian)
        {
            unlockedIngredients = unlockedIngredients!.Where(ingredient => ingredient.isVegetarian)
                .Where(ingredient => ingredient.isVegetarian).ToList();
        }
        
        for (var i = 0; i < numOfIngredients; i++)
        {
            // do something with likes and dislikes
            // for now its just random
            var ingredient = unlockedIngredients![Random.Range(0, unlockedIngredients.Count)];
            ingredients.Add(ingredient);
            unlockedIngredients.Remove(ingredient);
        }
            
        return ingredients;
    }

    /// <summary>
    /// Updates the order’s status (e.g., from Pending to InProgress or Completed).
    /// </summary>
    /// <param name="newStatus"></param>
    public void UpdateStatus(OrderStatus newStatus)
    {
        switch (newStatus)
        {
            case OrderStatus.Pending:
                status = newStatus;
                // update ui?
                break;
            case OrderStatus.InProgress:
                status = newStatus;
                // update ui?
                break;
            case OrderStatus.Completed:
                status = newStatus;
                // update ui?
                break;
            case OrderStatus.Failed:
                status = newStatus;
                // update ui?
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
        if (pizza.isPizzaCorrect)
        {
            accuracy = 100f;
        }
        else
        {
            var matchingIngredients = 0;
        
            // Loop through the player's pizza ingredients and check if they match the required ingredients
            foreach (var ingredient in pizza.IngredientsOnPizza)
            {
                // Check if the ingredient exists in the required ingredients list
                if (RequiredIngredients.Any(reqIngredient => reqIngredient.Equals(ingredient)))
                {
                    matchingIngredients++;
                }
            }
            
            // Calculate the accuracy score (e.g., percentage of correct ingredients)
            accuracy = (float)matchingIngredients / RequiredIngredients.Count;
            
            Debug.Log($"Pizza Evaluation: {matchingIngredients}/{RequiredIngredients.Count} ingredients match.");
            Debug.Log($"Accuracy Score: {accuracy * 100}%");
        }
        
        // remove some score if they are over the time limit
        if (isTimed && IsOrderTimedOut(orderTimer))
        {
            accuracy -= 50f;
        }

        accuracyScore =  accuracy;
    }

    /// <summary>
    /// Checks if the time for a timed order has expired.
    /// Returns true if the order's time limit has been exceeded.
    /// </summary>
    /// <param name="elapsedTime"></param>
    /// <returns></returns>
    public bool IsOrderTimedOut(float elapsedTime)
    {
        return isTimed && elapsedTime >= timeLimit;
    }

    /// <summary>
    /// Assigns a difficulty level to the order, impacting its complexity and scoring.
    /// </summary>
    /// <param name="difficulty"></param>
    public void SetOrderDifficulty(float difficulty)
    {
        
    }
}
