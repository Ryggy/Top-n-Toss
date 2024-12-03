using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReactionsUI : MonoBehaviour
{
    public GameObject reactionPrefab; // The prefab containing the TextMeshProUGUI component
    private TextMeshProUGUI reactionText; // Reference to the TextMeshProUGUI component
    private Transform currentCustomer; // Tracks the current customer's position
    public CustomerReactions reactions;
    public Camera mainCamera; // Reference to the main camera
    public RectTransform canvasRectTransform; // Reference to the canvas' RectTransform

    private void OnEnable()
    {
        DelegatesManager.Instance.OrderEventHandler.OnOrderSubmitted += ShowReaction;
    }

    private void OnDisable()
    {
        DelegatesManager.Instance.OrderEventHandler.OnOrderSubmitted -= ShowReaction;
    }

    private void Start()
    {
        reactions = GetComponent<CustomerReactions>();
        if (reactions == null)
        {
            Debug.LogError("Customer Reaction script is not assigned!");
            return;
        }

        if (reactionPrefab == null)
        {
            Debug.LogError("Reaction prefab is not assigned!");
            return;
        }

        // Get the TextMeshProUGUI component from the prefab
        reactionText = reactionPrefab.GetComponentInChildren<TextMeshProUGUI>();
        if (reactionText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in reaction prefab!");
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (canvasRectTransform == null)
        {
            Debug.LogError("Canvas RectTransform is not assigned!");
            return;
        }

        // Initially hide the reaction prefab
        reactionPrefab.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays a reaction at the customer's position based on the reward value.
    /// </summary>
    /// <param name="orderData">The order data containing the customer and reward information.</param>
    /// <param name="pizza">The pizza object (not used here but available if needed).</param>
    public void ShowReaction(OrderData orderData, Pizza pizza)
    {
        if (reactionPrefab == null || reactionText == null) return;

        // Get the customer's transform
        currentCustomer = CustomerManager.Instance.GetCustomerByID(orderData.CustomerData.CustomerID).transform;

        // Convert the customer's position to screen space
        Vector3 screenPos = mainCamera.WorldToScreenPoint(currentCustomer.position);

        // Convert the screen position to UI canvas space
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPos,
            mainCamera,
            out uiPos 
        );

        // Set the reaction prefab's position in the UI canvas
        reactionPrefab.GetComponent<RectTransform>().anchoredPosition = uiPos;

        // Get reaction text based on the reward value
        reactionText.text = GetReactionText(orderData.AccuracyScore);

        // Show the reaction
        reactionPrefab.gameObject.SetActive(true);

        // Optionally, hide the reaction after a delay
        StartCoroutine(HideReactionAfterDelay(2f)); // Adjust the delay time as needed
    }

    /// <summary>
    /// Determines the reaction text based on the reward value.
    /// </summary>
    /// <param name="reward">The reward value.</param>
    /// <returns>Reaction text string.</returns>
    private string GetReactionText(float reward)
    {
        if (reward <= 20) return reactions.horribleReactions[Random.Range(0, reactions.horribleReactions.Count)];
        if (reward <= 50) return reactions.badReactions[Random.Range(0, reactions.badReactions.Count)];
        if (reward <= 80) return reactions.okayReactions[Random.Range(0, reactions.okayReactions.Count)];
        if (reward <= 100) return reactions.goodReactions[Random.Range(0, reactions.goodReactions.Count)];
        return reactions.perfectReactions[Random.Range(0, reactions.perfectReactions.Count)];
    }

    /// <summary>
    /// Hides the reaction prefab after a delay.
    /// </summary>
    /// <param name="delay">Time in seconds to wait before hiding.</param>
    private IEnumerator HideReactionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        reactionPrefab.gameObject.SetActive(false);
    }
}
