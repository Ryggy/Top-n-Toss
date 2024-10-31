using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Customer : MonoBehaviour
{
    // The customer’s name, used for identification and UI purposes.
    public string customerName;

    public GameObject orderUIGameObject;
    [HideInInspector]
    public List<Transform> ingredientGameObjects = new List<Transform>();
    
    // The customer's order
    public List<Order> CustomerOrder = new List<Order>();
    // A list of ingredients the customer prefers on their pizza. These ingredients form part of the order's requirements.
    public List<Ingredient> PrefferedIngredients = new List<Ingredient>();
    // A list of ingredients the customer dislikes.
    // Adding these ingredients may reduce satisfaction or cause the order to fail.
    public List<Ingredient> DislikedIngredients = new List<Ingredient>();
    // The time a customer is willing to wait before they get frustrated and leave, affecting the time limit for the order.
    public float patience = 100f;
    // A value representing the customer’s current level of satisfaction.
    // It can change based on how well the player fulfils their order (correct ingredients, delivery time).
    public float satisfaction = 0f;
    // Indicates whether the customer is a VIP,
    // which may increase their order complexity and affect the rewards for completing their order.
    public bool isVIP = false;
    // Indicates whether the custer is a vegetarian
    // will limit the available amount of ingredients
    public bool isVegetarian = false;


    private void Start()
    {
        ingredientGameObjects = OrderUI.FindOrderIngredientGameObjects(this);
    }

    /// <summary>
    /// Generates a new order based on the customer’s preferred and disliked ingredients.
    /// This is used to communicate their pizza requirements to the player.
    /// </summary>
    /// <returns></returns>
    public List<Order> GenerateOrder()
    {
        // determine cumulative difficulty based on level config and if they're a VIP
        var cumulativeDifficulty = GetCumulativeDifficulty();
        // determine number of orders
        var numOfOrders = GetNumOfOrders(cumulativeDifficulty);
        // determine number of ingredients
        var numOfIngredients = GetNumOfIngredients(cumulativeDifficulty, numOfOrders);
        

        // create order/s
        for (int i = 0; i < numOfOrders; i++)
        {
            CustomerOrder.Add(new Order(this, numOfIngredients));
        }
        
        return CustomerOrder;
    }

    private int GetNumOfOrders(int cumulativeDifficulty)
    {
       // TODO: add formula to determine order scaling based on difficulty
       return 1;
    }

    private int GetNumOfIngredients(int cumulativeDifficulty, int numOfOrders)
    {
        // TODO: add formula to determine number of ingredients based on the difficulty and num of orders
        return 2;
    }

    private int GetCumulativeDifficulty()
    {
        // cumulative difficulty is sum of player level + VIP status (+ 2 diff)   
        return  Progression.Instance.currentLevel + (isVIP ? 2 : 0);
    }
    
    /// <summary>
    /// Reduces the customer's patience based on the time elapsed.
    /// If patience reaches zero, the customer leaves, resulting in a failed order.
    /// </summary>
    /// <param name="timeElapsed"></param>
    public void UpdatePatience(float timeElapsed)
    {
        if (!(patience <= 0)) return;
        
        patience -= timeElapsed;
    }

    /// <summary>
    /// Adjusts the customer’s satisfaction based on the pizza’s ingredients.
    /// Satisfaction will be higher if all preferred ingredients are included and no disliked ingredients are used.
    /// </summary>
    /// <param name="pizza"></param>
    public void UpdateSatisfaction(Pizza pizza)
    {
        // increase when liked ingredient
        foreach (var ingredient in pizza.IngredientsOnPizza)
        {
            // Check if the ingredient exists in the required ingredients list
            if (PrefferedIngredients.Any(reqIngredient => reqIngredient.Equals(ingredient)))
            {
                satisfaction += 20f;
            }
            // decrease when disliked ingredient
            if (DislikedIngredients.Any(reqIngredient => reqIngredient.Equals(ingredient)))
            {
                satisfaction -= 20f;
            }
            // increase for every ingredient (more ingredients = more satisfied)
            satisfaction += 5f;
        }
        
        // TODO: implement satisfaction upgrades here
        // other effects? 
    }

    /// <summary>
    /// Displays the customer’s reaction (happy, neutral, or angry)
    /// based on whether the order was successfully completed or failed
    /// </summary>
    /// <param name="orderSuccess"></param>
    public void ReactToOrder(bool orderSuccess)
    {
        
    }

    /// <summary>
    /// Calculates the reward based on the customer’s patience,
    /// satisfaction, whether they are a VIP and pizza accuracy
    /// </summary>
    /// <returns></returns>
    public int CalculateReward(int orderID)
    {
        var accuracyScore = CustomerOrder.Where(order => order.orderID == orderID).Sum(order => order.accuracyScore);

        return (int)(patience + satisfaction + (isVIP ? 100 : 0) + accuracyScore);
    }
}
