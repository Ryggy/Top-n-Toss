using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public GameObject onIcon;
    public GameObject offIcon;
    public GameObject offBackground;
    public GameObject onBackground;
    public RectTransform toggle;
    public float animationDuration = 0.5f; // Duration of the animation
    public Vector2 offPosition; // Panel's position when off
    public Vector2 onPosition; // Panel's position when om
    private bool isOn = false;
    
    public void OnToggle()
    {
        StopAllCoroutines(); // Stop any running animation
        if (isOn)
        {
            StartCoroutine(SlideToggleToPosition(offPosition));
        }
        else
        {
            StartCoroutine(SlideToggleToPosition(onPosition));
        }
        isOn = !isOn;
        onIcon.SetActive(isOn);
        offIcon.SetActive(!isOn);
    }
    
    private  IEnumerator SlideToggleToPosition(Vector2 targetPosition)
    {
        
        Vector2 startPosition = toggle.anchoredPosition;
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            toggle.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        toggle.anchoredPosition = targetPosition; // Snap to final position
        offBackground.SetActive(!isOn);
        onBackground.SetActive(isOn);
        

    }
}
