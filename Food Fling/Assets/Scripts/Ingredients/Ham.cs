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
            FoodType = FoodType.Meat;
            IngredientType = IngredientType.Topping;
            isUnlocked = true;
            isVegetarian = false;
            cost = 5;
            upgradeCost = 200;
        }
    }
}


