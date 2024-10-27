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
            quantity = 1;
            FoodType = FoodType.None;
            IngredientType = IngredientType.Sauce;
            isUnlocked = true;
            isVegetarian = true;
        }
    }
}


