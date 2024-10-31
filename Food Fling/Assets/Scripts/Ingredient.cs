using System;

public enum FoodType
{
    Fruit,
    Vegetable,
    Meat,
    Seafood,
    Dairy,
    None
}

public enum IngredientType
{
    Topping,
    Sauce,
    Cheese
}
[Serializable]
public class Ingredient
{
    public int ingredientID; 
    public string ingredientName;
    public IngredientType IngredientType;
    public FoodType FoodType;
    public bool isUnlocked;
    public bool isVegetarian;
    
    public int cost;
    public int upgradeCost;
    public void Unlock()
    {
        isUnlocked = true;
    }

    public void Lock()
    {
        isUnlocked = false;
    }
}
