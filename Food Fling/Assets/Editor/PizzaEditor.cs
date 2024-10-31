using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pizza))]
public class PizzaEditor : Editor
{
    string ingredientName;
    public override void OnInspectorGUI()
    {
        // Draw the default inspector to keep other properties visible
        DrawDefaultInspector();

        // Get a reference to the Pizza script
        Pizza pizza = (Pizza)target;

        ingredientName = EditorGUILayout.TextField("Ingredient Name (Case Sensitive)", ingredientName);
        
        // Add a button to check ingredient compatibility
        if (GUILayout.Button("Check Ingredient Compatibility"))
        {
            bool isCompatible = pizza.CheckIngredientCompatability(ingredientName);
            Debug.Log($"Ingredient compatibility: {isCompatible}");
        }
        
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
            foreach (var ingredient in pizza.IngredientsOnPizza)
            {
                Debug.Log($"{ingredient.ingredientName}");
            }
        }
        
        // Add a button to check if the pizza is ready
        if (GUILayout.Button("Is Pizza Correct?"))
        {
            bool isReady = pizza.IsPizzaCorrect();
            Debug.Log($"Pizza correct: {isReady}");
        }

        // Add a button to reset the pizza
        if (GUILayout.Button("Reset Pizza"))
        {
            pizza.ResetPizza();
        }
        
        // Add a button to reset the pizza
        if (GUILayout.Button("Submit Pizza"))
        {
            pizza.SubmitPizza(OrderManager.Instance.CurrentOrder.customerDetails);
        }
    }
}
