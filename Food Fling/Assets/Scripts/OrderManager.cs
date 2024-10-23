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
    
    // A list of currently active orders that need to be fulfilled.
    public List<Order> ActiveOrders = new List<Order>();
    // The current order being worked on by the player, containing customer preferences and required ingredients.
    public Order CurrentOrder;
    // Tracks the number of successfully completed orders during a game session.
    public int completedOrders = 0;
    // Tracks the number of failed orders during a game session, affecting player progression or rewards.
    public int failedOrders = 0;

    public Customer debugCustomer;
    public List<Customer> customers = new List<Customer>();
    private void Start()
    {
        InitialiseIngredientDictionary();

        foreach (var ingredient in AllIngredients)
        {
            Debug.Log($"initialised ingredient: {ingredient.Value.ingredientName}");
        }

        if (debugCustomer != null)
        {
            TakeCustomerOrder(debugCustomer);
        }
    }

    private void Update()
    {
        foreach (var order in ActiveOrders)
        {
            order.Update(Time.deltaTime);
            // if a customer has multiple orders this will get called twice
            // maybe update -> Customers -> update -> Orders
            order.customerDetails.UpdatePatience(Time.deltaTime);
        }
    }
    
    private void InitialiseIngredientDictionary()
    {
        AllIngredients.Add("Pepperoni", new Pepperoni());
        AllIngredients.Add("Cheese", new Cheese());
        AllIngredients.Add("Ham", new Ham());
        AllIngredients.Add("Pineapple", new Pineapple());
    }

    /// <summary>
    /// Generates a new order with specific customer preferences,
    /// ingredient requirements, and time limits based on the game’s difficulty and progression.
    /// </summary>
    public void TakeCustomerOrder(Customer customer)
    {
        // take the customers order
        var orders = customer.GenerateOrder();
        // keep a record of what they want
        //ActiveOrders.AddRange(orders);
        // display the orders
        foreach (var order in ActiveOrders)
        {
            Debug.Log("Order Up: A Pizza with ");
            foreach (var t in order.RequiredIngredients)
            {
                Debug.Log($"{t.ingredientName}");
            }
        }
        
        // TODO: implement system for selecting and assigning orders
        // assign the first order for now
        AssignOrderToPlayer(ActiveOrders[0].orderID);
    }
    
    /// <summary>
    /// Assigns the generated order to the player, setting it as the CurrentOrder and resetting the OrderTimer.
    /// <param name="orderID"></param>
    /// </summary>
    public void AssignOrderToPlayer(int orderID)
    {
        // set the current order via order ID
        CurrentOrder = GetActiveOrderByID(orderID);
        CurrentOrder?.UpdateStatus(OrderStatus.InProgress);
    }
    
    /// <summary>
    /// Marks the order as completed, calculates rewards, and updates the CompletedOrders count.
    /// </summary>
    public void CompleteOrder(int orderID)
    {
        completedOrders++;
        Order tempOrder = GetActiveOrderByID(orderID);
        tempOrder.UpdateStatus(OrderStatus.Completed);
        ActiveOrders.Remove(tempOrder);
        Debug.Log($"Completed Order: {orderID}");
        
        // taking another random customers order
        TakeCustomerOrder(customers[Random.Range(0,customers.Count)]);
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
        foreach (var order in ActiveOrders.Where(order => order.orderID == orderID))
        {
            return order;
        }
        
        Debug.LogWarning($"No order found in active orders list with ID: {orderID}");
        return null;
    }
}
