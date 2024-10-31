using Unity.VisualScripting;
using UnityEngine;

namespace Ingredients
{
    public class BBQSauce : Ingredient
    {
        public BBQSauce()
        {
            ingredientID = 5;
            ingredientName = "BBQSauce";
            FoodType = FoodType.None;
            IngredientType = IngredientType.Sauce;
            isUnlocked = false;
            isVegetarian = true;
            cost = 5;
            upgradeCost = 200;
        }
    }
}


