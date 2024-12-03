using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Customer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    public CustomerData Data = new CustomerData();
    public GameObject orderUIGameObject;
    public MultiplierManager MultiplierManager;
    [HideInInspector]
    public List<Transform> ingredientGameObjects = new List<Transform>();
    [SerializeField]
    public List<Order> CustomerOrder = new List<Order>();
    
    private void Start()
    {

        ingredientGameObjects = OrderUI.FindOrderIngredientGameObjects(this);
        
        if (MultiplierManager == null)
        {
           MultiplierManager = FindObjectOfType<MultiplierManager>();
        }
    }

    private void RemoveCustomerOrder(OrderData order, bool success)
    {
       var tempOrder = OrderManager.Instance.GetActiveOrderByID(order.OrderID);

       if (CustomerOrder.Contains(tempOrder))
       {
           CustomerOrder.Remove(tempOrder);
       }
    }

    private void OnEnable()
    {
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerArrives += HandleCustomerArrives;
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerLeaves += HandleCustomerLeaves;
        DelegatesManager.Instance.OrderEventHandler.OnOrderCompleted += RemoveCustomerOrder;
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerArrives -= HandleCustomerArrives;
        DelegatesManager.Instance.CustomerEventHandler.OnCustomerLeaves -= HandleCustomerLeaves;
        DelegatesManager.Instance.OrderEventHandler.OnOrderCompleted -= RemoveCustomerOrder;
    }

    /// <summary>
    /// Moves the customer to a target position and triggers a callback upon arrival.
    /// </summary>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="onArrival">Callback triggered when movement is complete.</param>
    /// <returns></returns>
    public IEnumerator MoveToPosition(Vector3 targetPosition, Action onArrival = null)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        onArrival?.Invoke();
    }
    
    private void HandleCustomerArrives(CustomerData customer)
    {
        // Handle customer arrival logic
        AudioManager.Instance.PlaySoundEffectByIndex(0);
        Debug.Log($"Customer {customer.CustomerName} has arrived.");
    }

    private void HandleCustomerLeaves(CustomerData customer)
    {
        // Handle customer leaving logic
        AudioManager.Instance.PlaySoundEffectByIndex(1);
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
        //var numOfOrders = GetNumOfOrders(cumulativeDifficulty);
        var numOfOrders = 1;
        var numOfIngredients = GetNumOfIngredients(cumulativeDifficulty, 1);

        for (int i = 0; i < numOfOrders; i++)
        {
            CustomerOrder.Add(new Order(Data, MultiplierManager, numOfIngredients));
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
        int minIngredients = 2; // Minimum number of ingredients
        int maxIngredients = 6; // Maximum number of ingredients
        int difficultyDivisor = 3; // Controls scaling rate

        // Formula to calculate number of ingredients
        int numOfIngredients = minIngredients + (cumulativeDifficulty / difficultyDivisor);

        // Clamp the value between min and max
        return Mathf.Clamp(numOfIngredients, minIngredients, maxIngredients);
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
