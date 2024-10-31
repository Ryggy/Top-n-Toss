using Unity.VisualScripting;
using UnityEngine;

namespace Ingredients
{
    public class Pepperoni : Ingredient
    {
        public Pepperoni()
        {
            ingredientID = 1;
            ingredientName = "Pepperoni";
            FoodType = FoodType.Meat;
            IngredientType = IngredientType.Topping;
            isUnlocked = false;
            isVegetarian = false;
            cost = 5;
            upgradeCost = 200;
        }
    }
}


