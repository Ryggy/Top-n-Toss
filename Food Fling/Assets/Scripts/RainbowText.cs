using UnityEngine;
using TMPro;

public class RainbowText : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // Reference to the TMP text component
    public float cycleSpeed = 1.0f; // Speed of the color cycle
    [Range(0, 1)] public float saturation = 0.5f; // Saturation level for the colors
    [Range(0, 1)] public float brightness = 0.75f; // Brightness level for the colors

    private float hue = 0.0f; // Hue value for cycling through colors

    void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        // Increment hue over time based on cycle speed
        hue += Time.deltaTime * cycleSpeed;

        // Ensure hue stays within 0-1 range
        if (hue > 1.0f)
        {
            hue -= 1.0f;
        }

        // Convert hue to a color with the specified saturation and brightness, and set the text color
        Color rainbowColor = Color.HSVToRGB(hue, saturation, brightness);
        tmpText.color = rainbowColor;
    }
}