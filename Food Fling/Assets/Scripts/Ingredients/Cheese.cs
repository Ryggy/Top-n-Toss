using Unity.VisualScripting;
using UnityEngine;

namespace Ingredients
{
    public class Cheese : Ingredient
    {
        public Cheese()
        {
            ingredientID = 3;
            ingredientName = "Cheese";
            FoodType = FoodType.Dairy;
            IngredientType = IngredientType.Cheese;
            isUnlocked = true;
            isVegetarian = true;
            cost = 5;
            upgradeCost = 200;
        }
    }
}


