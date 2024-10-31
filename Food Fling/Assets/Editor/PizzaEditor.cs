using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pizza))]
public class PizzaEditor : Editor
{
    string ingredientName;
    private string customerID;
    public override void OnInspectorGUI()
    {
        // Draw the default inspector to keep other properties visible
        DrawDefaultInspector();

        // Get a reference to the Pizza script
        Pizza pizza = (Pizza)target;

        ingredientName = EditorGUILayout.TextField("Ingredient Name (Case Sensitive)", ingredientName);
        
        
        // Add a button to add an ingredient
        if (GUILayout.Button("Add Ingredient"))
        {
            pizza.AddIngredient(ingredientName);
        }

        // Add a button to remove an ingredient
        if (GUILayout.Button("Remove Ingredient"))
        {
            pizza.RemoveIngredient(ingredientName);
        }
        
        // Add a button to remove an ingredient
        if (GUILayout.Button("Ingredients on Pizza?"))
        {
            Debug.Log("The Pizza has: ");
            foreach (var ingredient in pizza.Data.IngredientsOnPizza)
            {
                Debug.Log($"{ingredient.ingredientName}");
            }
        }
        

        // Add a button to reset the pizza
        if (GUILayout.Button("Reset Pizza"))
        {
            pizza.ResetPizza();
        }
        
        customerID = EditorGUILayout.TextField("Submit Customer ID", customerID);
        
        // Add a button to submit the pizza
        if (GUILayout.Button("Submit Pizza"))
        {
            pizza.SubmitPizza(CustomerManager.Instance.GetCustomerByID(Int32.Parse(customerID)));
        }
    }
}
