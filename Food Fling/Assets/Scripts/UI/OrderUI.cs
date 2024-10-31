using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderUI
{
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
    
    public static void DisplayOrder(Customer customer ,int orderID)
    {
        var order = OrderManager.Instance.GetActiveOrderByID(orderID);
        
        foreach (var ingredientUI in order.RequiredIngredients.SelectMany(ingredient => 
                     customer.ingredientGameObjects.Where(ingredientUI =>
                     ingredient.ingredientName == ingredientUI.name)))
        {
            ingredientUI.gameObject.SetActive(true);
        }
    }

    public static void ResetOrderUI(Customer customer, int orderID)
    {
        var order = OrderManager.Instance.GetActiveOrderByID(orderID);
        
        foreach (var ingredient in customer.ingredientGameObjects)
        {
            ingredient.gameObject.SetActive(false);
        }
    }
}
