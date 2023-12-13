using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    private Collider objectCollider;
    private Renderer objectRenderer;
    private Material originalMaterial;
    public Material transparentMaterial;

    void Start()
    {
        objectCollider = GetComponent<Collider>();
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    void Update()
    {
        // Make transparent while space bar is held down
        if (Input.GetKey(KeyCode.Space))
        {
            objectCollider.enabled = false;
            objectRenderer.material = transparentMaterial;
        }

        // Return to normal when space bar is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            objectCollider.enabled = true;
            objectRenderer.material = originalMaterial;
        }
    }
}
