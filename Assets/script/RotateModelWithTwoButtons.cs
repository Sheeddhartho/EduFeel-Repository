using UnityEngine;
using UnityEngine.UI;

public class RotateModelWithTwoButtons : MonoBehaviour
{
    public GameObject modelToRotate;  // Reference to the 3D model that will be rotated
    public Button rotateLeftButton;   // Button for rotating the model to the left (counterclockwise)
    public Button rotateRightButton;  // Button for rotating the model to the right (clockwise)
    private float rotationAngle = 90f;  // The rotation angle (90 degrees)

    void Start()
    {
        // Ensure both buttons and the model are set in the Inspector
        if (rotateLeftButton != null && rotateRightButton != null && modelToRotate != null)
        {
            // Add listeners to the buttons
            rotateLeftButton.onClick.AddListener(RotateLeft);
            rotateRightButton.onClick.AddListener(RotateRight);
        }
        else
        {
            Debug.LogError("One or more references are missing in the Inspector.");
        }
    }

    // Method to rotate the model to the left (counterclockwise) by 90 degrees on the Y-axis
    void RotateLeft()
    {
        // Rotate left by 90 degrees (negative rotation on the Y-axis)
        modelToRotate.transform.Rotate(0f, -rotationAngle, 0f);
        Debug.Log("Model rotated 90 degrees to the left (counterclockwise) on the Y-axis.");
    }

    // Method to rotate the model to the right (clockwise) by 90 degrees on the Y-axis
    void RotateRight()
    {
        // Rotate right by 90 degrees (positive rotation on the Y-axis)
        modelToRotate.transform.Rotate(0f, rotationAngle, 0f);
        Debug.Log("Model rotated 90 degrees to the right (clockwise) on the Y-axis.");
    }
}
