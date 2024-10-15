using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum FoodType
{
    Fruit,
    Vegetable,
    Meat,
    Seafood,
    Dairy
}

public enum IngredientType
{
    Topping,
    Sauce,
    Cheese
}
public class Ingredient
{
    // Unique identifier for the ingredient.
    public int ingredientID;
    // The name of the ingredient (e.g., "Pepperoni", "Mushroom")
    public string ingredientName;
    // The amount of the ingredient
    public int quantity;
    // The type of the ingredient (e.g., Topping, Sauce, Cheese). 
    public IngredientType IngredientType;
    // The type of the food (e.g., Vegetable, Meat, Dairy). 
    public FoodType FoodType;
    // Indicates whether the ingredient is available for selection by the player.
    public bool isUnlocked;
    // Indicates whether the ingredient is vegetarian.
    public bool isVegetarian;
    //public List<SpecialEffect> SpecialEffects;

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void Lock()
    {
        isUnlocked = false;
    }

    // void AddSpecialEffect(SpecialEffect effect)
    // {
    //     SpecialEffects.Add(effect)
    // }
    
    // void RemoveSpecialEffect(SpecialEffect effect)
    // {
    //     SpecialEffects.Remove(effect)
    // }
}
