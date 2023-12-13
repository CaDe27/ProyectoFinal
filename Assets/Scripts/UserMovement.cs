using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float keyMoveSpeed = 5.0f; // Speed of movement when using keys
    public float rotationSpeed = 2.0f; // Speed of rotation when dragging with mouse

    private Vector3 lastMousePosition; // To keep track of the last mouse position

    void Update()
    {
        // Keyboard movement
        float horizontal = Input.GetAxis("Horizontal") * keyMoveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * keyMoveSpeed * Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);

        // Mouse drag rotation
        if (Input.GetMouseButtonDown(0)) // When the left mouse button is pressed
        {
            lastMousePosition = Input.mousePosition; // Store the current mouse position
        }

        if (Input.GetMouseButton(0)) // If the left mouse button is held down
        {
            Vector3 delta = Input.mousePosition - lastMousePosition; // Calculate the movement difference
            lastMousePosition = Input.mousePosition; // Update the last mouse position to the new position

            // Apply the rotation
            float rotationX = delta.y * rotationSpeed * Time.deltaTime;
            float rotationY = delta.x * rotationSpeed * Time.deltaTime;

            transform.eulerAngles += new Vector3(-rotationX, rotationY, 0);
        }
    }
}
