using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DragAndDrop : MonoBehaviour
{
    public GameObject SelectedIngredientContainer;
    private GameObject currentIngredient;
    public GameObject pizzaGo;
    public LayerMask ingredientLayer;
    public LayerMask pizzaLayer;
    private Vector3 ingredientStartPos;
    private InputActions inputActions;
    
    private Camera mainCamera;
    private bool isDragging = false;       // Track if we're actively dragging an ingredient
    //private int ingredientCount = 0;

    private Pizza pizzaScript;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        inputActions = new InputActions();
        
        // Assign action callbacks
        inputActions.PizzaMaking.Select.performed += OnSelect;
        inputActions.PizzaMaking.Select.canceled += OnRelease;
        inputActions.PizzaMaking.Drag.performed += OnDrag;
        
        // Get reference to the Pizza component from the pizza GameObject
        if (pizzaGo != null)
        {
            pizzaScript = pizzaGo.GetComponent<Pizza>();
        }
    }

    private void OnEnable()
    {
        inputActions.PizzaMaking.Enable();
    }

    private void OnDisable()
    {
        inputActions.PizzaMaking.Disable();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (currentIngredient != null) return; // Prevent selecting multiple ingredients
        
        Vector2 touchPos = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        
        Debug.Log("Touch Detected");
        
        // Try selecting an ingredient
        if (currentIngredient == null)
        {
            Collider2D hit = Physics2D.OverlapPoint(touchPos, ingredientLayer);
            if (hit != null && hit.CompareTag("Ingredient"))
            {

                if (hit.gameObject.transform.parent == pizzaGo.transform)
                {
                    SelectedIngredient(hit.gameObject, touchPos);
                    return;
                }
                
                SelectNewIngredient(hit.gameObject, touchPos);
                Debug.Log("Selecting Ingredient : "+ hit.gameObject.name);
            }
        }
    }

    private void SelectedIngredient(GameObject hitGameObject, Vector2 touchPos)
    {
        pizzaScript.RemoveIngredient(hitGameObject.name);
        SelectNewIngredient(hitGameObject, touchPos);
        Destroy(hitGameObject);
    }

    public void OnRelease(InputAction.CallbackContext context)
    {
        if (isDragging && currentIngredient != null)
        {
            Vector3 touchPos = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            // Check if the current ingredient is over the pizza
            Collider2D pizzaHit = Physics2D.OverlapPoint(touchPos, pizzaLayer);
            if (pizzaHit != null && pizzaHit.gameObject == pizzaGo)
            {
                // If over the pizza, add the ingredient to the pizza and stop dragging
                AddIngredientToPizza();
                Debug.Log("Adding Ingredient To Pizza");
            }
            else
            {
                // If not over the pizza, destroy the ingredient
                Debug.Log("No Pizza Found, Destroying Ingredient: "+currentIngredient.name);
                Destroy(currentIngredient);
            }
            isDragging = false;  // Stop dragging after releasing
            currentIngredient = null;
        }
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        if (isDragging && currentIngredient != null)
        {
            Vector2 touchPos = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            currentIngredient.transform.position = touchPos;  // Move the ingredient with the drag
        }
    }

    private void SelectNewIngredient(GameObject ingredient, Vector3 position)
    {
        // Instantiate the ingredient at the touch/mouse position
        // add an offset so its in front of the other ingredients
        currentIngredient = Instantiate(ingredient, position, Quaternion.identity, SelectedIngredientContainer.transform);
        ingredientStartPos = currentIngredient.transform.position;
        isDragging = true;  // Begin dragging when an ingredient is selected
    }

    private void AddIngredientToPizza()
    {
        // Attach the ingredient to the pizza by making it a child of the pizza GameObject
        currentIngredient.transform.SetParent(pizzaGo.transform);
        //currentIngredient.transform.position += new Vector3(0, 0, -ingredientCount);
        isDragging = false;  // Stop dragging once the ingredient is added to the pizza
        //ingredientCount++;
        
        // Remove the "(Clone)" part from the name
        string originalName = currentIngredient.name;
        currentIngredient.name = originalName.Replace("(Clone)", "").Trim();
        pizzaScript.AddIngredient(currentIngredient.name);
    }
}
    