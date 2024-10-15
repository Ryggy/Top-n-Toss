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
            quantity = 1;
            FoodType = FoodType.Meat;
            IngredientType = IngredientType.Topping;
            isUnlocked = true;
            isVegetarian = false;
        }
    }
}


