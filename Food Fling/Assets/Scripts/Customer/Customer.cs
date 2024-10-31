using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Customer : MonoBehaviour
{
    [SerializeField] 
    public CustomerData Data = new CustomerData();
    public GameObject orderUIGameObject;
    
    [HideInInspector]
    public List<Transform> ingredientGameObjects = new List<Transform>();
    [SerializeField]
    public List<Order> CustomerOrder = new List<Order>();

    private void Awake()
    {
        CustomerManager.Instance.AddCustomer(this); 
    }

    private void Start()
    {

        ingredientGameObjects = OrderUI.FindOrderIngredientGameObjects(this);
        
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerArrives += HandleCustomerArrives;
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerLeaves += HandleCustomerLeaves;
        DelegatesManager.Instance.OrderEventHandler.OnOrderCompleted += RemoveCustomerOrder;
    }

    private void RemoveCustomerOrder(OrderData order, bool success)
    {
       var tempOrder = OrderManager.Instance.GetActiveOrderByID(order.OrderID);

       if (CustomerOrder.Contains(tempOrder))
       {
           CustomerOrder.Remove(tempOrder);
       }
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerArrives -= HandleCustomerArrives;
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerLeaves -= HandleCustomerLeaves;
        DelegatesManager.Instance.OrderEventHandler.OnOrderCompleted -= RemoveCustomerOrder;
    }

    private void HandleCustomerArrives(CustomerData customer)
    {
        // Handle customer arrival logic
        Debug.Log($"Customer {customer.CustomerName} has arrived.");
    }

    private void HandleCustomerLeaves(CustomerData customer)
    {
        // Handle customer leaving logic
        Debug.Log($"Customer {customer.CustomerName} has left.");
    }
    
    /// <summary>
    /// Generates a new order based on the customer’s preferred and disliked ingredients.
    /// This is used to communicate their pizza requirements to the player.
    /// </summary>
    /// <returns></returns>
    public List<Order> GenerateOrder()
    {
        var cumulativeDifficulty = GetCumulativeDifficulty();
        var numOfOrders = GetNumOfOrders(cumulativeDifficulty);
        var numOfIngredients = GetNumOfIngredients(cumulativeDifficulty, numOfOrders);

        for (int i = 0; i < numOfOrders; i++)
        {
            CustomerOrder.Add(new Order(Data, numOfIngredients));
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
        return Progression.Instance.Data.CurrentLevel + (Data.IsVIP ? 2 : 0);
    }
    
    /// <summary>
    /// Reduces the customer's patience based on the time elapsed.
    /// If patience reaches zero, the customer leaves, resulting in a failed order.
    /// </summary>
    /// <param name="timeElapsed"></param>
    public void UpdatePatience(float timeElapsed)
    {
        Data.Patience -= timeElapsed;
        DelegatesManager.Instance.TriggerOnCustomerPatienceChange(Data);
    }

    /// <summary>
    /// Adjusts the customer’s satisfaction based on the pizza’s ingredients.
    /// Satisfaction will be higher if all preferred ingredients are included and no disliked ingredients are used.
    /// </summary>
    /// <param name="pizza"></param>
    public void UpdateSatisfaction(Pizza pizza)
    {
        foreach (var ingredient in pizza.Data.IngredientsOnPizza)
        {
            if (Data.PreferredIngredients.Any(reqIngredient => reqIngredient.Equals(ingredient)))
            {
                Data.Satisfaction += 20f;
            }
            if (Data.DislikedIngredients.Any(reqIngredient => reqIngredient.Equals(ingredient)))
            {
                Data.Satisfaction -= 20f;
            }
            Data.Satisfaction += 5f;
        }
        
        DelegatesManager.Instance.TriggerOnCustomerSatisfactionChange(Data);
    }
}
