using Unity.VisualScripting;
using UnityEngine;

namespace Ingredients
{
    public class Ham : Ingredient
    {
        public Ham()
        {
            ingredientID = 2;
            ingredientName = "Ham";
            quantity = 1;
            FoodType = FoodType.Meat;
            IngredientType = IngredientType.Topping;
            isUnlocked = true;
            isVegetarian = false;
        }
    }
}


