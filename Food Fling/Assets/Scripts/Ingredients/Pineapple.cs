using Unity.VisualScripting;
using UnityEngine;

namespace Ingredients
{
    public class Pineapple : Ingredient
    {
        public Pineapple()
        {
            ingredientID = 0;
            ingredientName = "Pineapple";
            quantity = 1;
            FoodType = FoodType.Fruit;
            IngredientType = IngredientType.Topping;
            isUnlocked = false;
            isVegetarian = true;
        }
    }
}


