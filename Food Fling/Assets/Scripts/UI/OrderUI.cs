using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderUI : MonoBehaviour
{
    private void OnEnable()
    {
        DelegatesManager.Instance.OrderEventHandler.OnOrderGenerated += DisplayOrder;
        DelegatesManager.Instance.OrderEventHandler.OnOrderCompleted += ResetOrderUI;
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.OrderEventHandler.OnOrderGenerated -= DisplayOrder;
        DelegatesManager.Instance.OrderEventHandler.OnOrderCompleted -= ResetOrderUI;
    }
    
    
    public static List<Transform> FindOrderIngredientGameObjects(Customer customer)
    {
        var ingredientGameObjects = new List<Transform>();
        
        for (int i = 0; i < customer.orderUIGameObject.transform.childCount; i++)
        {
            var childGo = customer.orderUIGameObject.transform.GetChild(i);
            if (childGo.CompareTag("Ingredient"))
            {
                ingredientGameObjects.Add(childGo);
            }
        }

        return ingredientGameObjects;
    }

    private static void DisplayOrder(OrderData order)
    {
        Debug.Log($"Displaying Order: {order.OrderID}");
       var customer = CustomerManager.Instance.GetCustomerByID(order.CustomerData.CustomerID);
        // find all the ingredient UI gameobjects with matching names of the required ingredients
        // and set them to active
        foreach(var ingredientUI in order.RequiredIngredients.SelectMany(ingredient => 
                     customer.ingredientGameObjects.Where(ingredientUI =>
                     ingredient.ingredientName == ingredientUI.name)))
        {
            ingredientUI.gameObject.SetActive(true);
        }
    }

    private static void ResetOrderUI(OrderData order, bool success)
    {
        var customer = CustomerManager.Instance.GetCustomerByID(order.CustomerData.CustomerID);
        
        foreach (var ingredient in customer.ingredientGameObjects)
        {
            ingredient.gameObject.SetActive(false);
        }
    }
}
