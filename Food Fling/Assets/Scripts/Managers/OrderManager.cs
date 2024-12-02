using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ingredients;
using UnityEngine;
using Random = UnityEngine.Random;

public enum OrderStatus
{
    Pending,
    InProgress,
    Completed,
    Failed
}
public sealed class OrderManager : MonoBehaviour
{
    // singleton
    private static OrderManager _instance = null;
    public static OrderManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // Try to find an existing instance in the scene
            _instance = FindObjectOfType<OrderManager>();
                
            if (_instance == null)
            {
                Debug.LogError("No OrderManager found in the scene!");
            }
            return _instance;
        }
    }
    public readonly Dictionary<string, Ingredient> AllIngredients = new Dictionary<string, Ingredient>();

    public Dictionary<int, Order> ActiveOrders = new Dictionary<int, Order>();
    
    
    // A list of currently active orders that need to be fulfilled.
    //public List<Order> ActiveOrders = new List<Order>();
    
    // The current order being worked on by the player, containing customer preferences and required ingredients.
    //public Order CurrentOrder;
    // Tracks the number of successfully completed orders during a game session.
    public int completedOrders = 0;
    // Tracks the number of failed orders during a game session, affecting player progression or rewards.
    public int failedOrders = 0;

    private void Awake()
    {
        InitialiseIngredientDictionary();

        foreach (var ingredient in AllIngredients)
        {
            Debug.Log($"initialised ingredient: {ingredient.Value.ingredientName}");
        }
        
        foreach (var ingredient in AllIngredients.Where(ingredient => ingredient.Value.isUnlocked))
        {
            Progression.Instance.Data.UnlockedIngredients.Add(ingredient.Key);
        }
    }
    
    private void Update()
    {
        foreach(var order in ActiveOrders)
        {
            order.Value.UpdateOrder(Time.deltaTime);
        }
    }
    
    private void InitialiseIngredientDictionary()
    {
        AllIngredients.Add("Pepperoni", new Pepperoni());
        AllIngredients.Add("Cheese", new Cheese());
        AllIngredients.Add("Ham", new Ham());
        AllIngredients.Add("Pineapple", new Pineapple());
        AllIngredients.Add("BBQSauce", new BBQSauce());
        AllIngredients.Add("TomatoSauce", new TomatoSauce());
    }

    /// <summary>
    /// Generates a new order with specific customer preferences,
    /// ingredient requirements, and time limits based on the game’s difficulty and progression.
    /// </summary>
    public void TakeCustomerOrder(Customer customer)
    {
        // take the customers order
        var orders = customer.GenerateOrder();

        foreach (var order in orders)
        {
            ActiveOrders.Add(order.Data.OrderID, order);
            order.UpdateStatus(OrderStatus.InProgress);
        }
    }
    
    /// <summary>
    /// Marks the order as completed, calculates rewards, and updates the CompletedOrders count.
    /// </summary>
    public void CompleteOrder(int orderID)
    {
        completedOrders++;
        Order tempOrder = GetActiveOrderByID(orderID);
        tempOrder.UpdateStatus(OrderStatus.Completed);
        ActiveOrders.Remove(orderID);
        Debug.Log($"Completed Order: {orderID}");
        
        // Taking another random customer's order
        var allCustomers = CustomerManager.Instance.Customers.Values.ToList();
        if(allCustomers.Count > 0)
        {
            var randomCustomer = allCustomers[Random.Range(0, allCustomers.Count)];
            TakeCustomerOrder(randomCustomer);
        }
    }

    /// <summary>
    /// Marks the order as failed due to incorrect ingredients or running out of time, incrementing the FailedOrders count.
    /// </summary>
    public void FailOrder(int orderID)
    {
        failedOrders++;
        GetActiveOrderByID(orderID).UpdateStatus(OrderStatus.Failed);
        Debug.Log($"Failed Order: {orderID}");
    }

    public Order GetActiveOrderByID(int orderID)
    {
        if (ActiveOrders.TryGetValue(orderID, out var order))
        {
            return order;
        }
        
        Debug.LogWarning($"No order found in active orders list with ID: {orderID}");
        return null;
    }
}
