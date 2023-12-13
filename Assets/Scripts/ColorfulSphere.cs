using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulSphere : MonoBehaviour
{
    private Renderer rend;
    private Color currentColor;
    private Color targetColor;
    public float changeInterval = 2.0f; // Interval in seconds to change color
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        currentColor = rend.material.color; // Initialize current color
        targetColor = GenerateVibrantColor();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if( timer >= changeInterval){
            timer = 0.0f; 
            currentColor = targetColor;
            targetColor = GenerateVibrantColor();
        }

        rend.material.color = Color.Lerp(currentColor, targetColor, timer / changeInterval);
    }

    Color GenerateVibrantColor()
    {
        // HSV color model is more suitable for generating vibrant colors
        // Random Hue, full Saturation (1), and full Value (1) for vibrant colors
        float hue = Random.Range(0.0f, 1.0f);
        return Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
    
}
