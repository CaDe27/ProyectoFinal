using UnityEngine;
using TMPro;

public class ReflectorUpdate : MonoBehaviour
{
    public TMP_InputField inputField;

    public void UpdateCoordinates()
    {
        if (inputField == null || inputField.text == "")
        {
            return;
        }

        // Parse the input text
        string[] coordinates = inputField.text.Split(',');

        if (coordinates.Length != 3)
        {
            Debug.LogError("Input must be in the format 'x, y, z'.");
            return;
        }

        float x, y, z;
        if (float.TryParse(coordinates[0].Trim(), out x) &&
            float.TryParse(coordinates[1].Trim(), out y) &&
            float.TryParse(coordinates[2].Trim(), out z))
        {
            // Update the position of the GameObject
            transform.position = new Vector3(x, y, z);
        }
        else
        {
            Debug.LogError("Invalid input. Make sure to enter valid numbers.");
        }
    }
}
