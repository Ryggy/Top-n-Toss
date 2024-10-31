using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public RectTransform panel; // The panel to be animated
    public float animationDuration = 0.5f; // Duration of the animation
    public Vector2 closedPosition; // Panel's position when closed
    public Vector2 openPosition; // Panel's position when open
    private bool isOpen = false; // Tracks if the panel is currently open

    private void Start()
    {
        // Initialise the panel at the closed position
        panel.anchoredPosition = closedPosition;
    }

    public void TogglePanel()
    {
        StopAllCoroutines(); // Stop any running animation
        if (isOpen)
        {
            StartCoroutine(SlidePanelToPosition(closedPosition));
        }
        else
        {
            StartCoroutine(SlidePanelToPosition(openPosition));
        }
        isOpen = !isOpen;
    }

    private System.Collections.IEnumerator SlidePanelToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition; // Snap to final position
    }
}
