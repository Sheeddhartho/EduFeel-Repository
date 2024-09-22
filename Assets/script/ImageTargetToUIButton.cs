using UnityEngine;
using Vuforia;  // Ensure Vuforia Engine is imported
using UnityEngine.UI;  // Import Unity UI for button interaction

public class ImageTargetToUIButton : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;  // Reference to the Image Target Observer
    public Button myButton;  // Reference to the UI button to be clicked

    void Start()
    {
        // Get the ObserverBehaviour (used in the latest Vuforia versions)
        mObserverBehaviour = GetComponent<ObserverBehaviour>();

        // Ensure the Image Target has been set up properly
        if (mObserverBehaviour)
        {
            // Register a callback when the target's status changes
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    // Callback method to handle when the tracking state changes
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        // Check if the Image Target is being tracked
        if (targetStatus.Status == Status.TRACKED ||
            targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // Trigger the button click when the target is tracked
            TriggerButtonClick();
        }
    }

    // Trigger the button's onClick event
    private void TriggerButtonClick()
    {
        if (myButton != null)
        {
            Debug.Log("Image Target detected! Button Click triggered.");
            myButton.onClick.Invoke();  // Programmatically trigger the button click
        }
    }
}
