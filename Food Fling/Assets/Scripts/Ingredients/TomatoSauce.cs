using Unity.VisualScripting;
using UnityEngine;

namespace Ingredients
{
    public class TomatoSauce : Ingredient
    {
        public TomatoSauce()
        {
            ingredientID = 4;
            ingredientName = "TomatoSauce";
            quantity = 1;
            FoodType = FoodType.None;
            IngredientType = IngredientType.Sauce;
            isUnlocked = true;
            isVegetarian = true;
        }
    }
}


