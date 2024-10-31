using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaData
{
    public List<Ingredient> IngredientsOnPizza { get; set; } = new List<Ingredient>();
    public int MaxIngredients { get; set; } = 5;
}