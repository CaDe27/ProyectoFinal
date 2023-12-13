using UnityEngine;

public class ColorOscillator : MonoBehaviour
{
    public Color baseColor;
    public float colorRange = 0.5f; // Range of the oscillation (0 = no change, 1 = full range from black to white)
    public float oscillationSpeed = 0.5f; // Speed of color oscillation

    private Renderer rend;
    private float lightnessOffset;
    private float originalTransparency; 

    void Start()
    {
        rend = GetComponent<Renderer>();

        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        //rend.material.renderQueue = 3000;

        baseColor = GenerateVibrantColor();
        
        originalTransparency = Random.Range(0.6f, 0.8f);
        Color transparentColor = new Color(baseColor.r, baseColor.g, baseColor.b, originalTransparency);
        rend.material.color = transparentColor;
    }

    void Update()
    {
        // Oscillate the lightness of the color
        lightnessOffset = Mathf.PingPong(Time.time * oscillationSpeed, colorRange);

        // Apply the oscillating color
        Color newColor = AdjustColorBrightness(baseColor, lightnessOffset);
        newColor.a = originalTransparency;
        rend.material.color = newColor;
    }

    Color AdjustColorBrightness(Color color, float brightnessFactor)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v); // Convert to HSV
        v = Mathf.Clamp01(v + brightnessFactor * colorRange - colorRange / 2); // Adjust value/brightness
        return Color.HSVToRGB(h, s, v); // Convert back to RGB
    }

    Color GenerateVibrantColor()
    {
        // HSV color model is more suitable for generating vibrant colors
        // Random Hue, full Saturation (1), and full Value (1) for vibrant colors
        float hue = Random.Range(0.0f, 1.0f);
        return Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
