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
            FoodType = FoodType.None;
            IngredientType = IngredientType.Sauce;
            isUnlocked = true;
            isVegetarian = true;
            cost = 5;
            upgradeCost = 200;
        }
    }
}


