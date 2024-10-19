using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public GameObject objectToEnable;
    public GameObject objectToDisable;
    public Text buttonText;

    private bool isObjectEnabled = true;

    private void Start()
    {
        // Initialize the button text based on the initial state
        UpdateButtonText();
    }

    public void ToggleObjects()
    {
        // Toggle the enable/disable state of objects
        isObjectEnabled = !isObjectEnabled;

        if (objectToEnable != null)
            objectToEnable.SetActive(isObjectEnabled);

        if (objectToDisable != null)
            objectToDisable.SetActive(!isObjectEnabled);

        // Update the button text to reflect the new state
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        // Update the button text to show the current state
        //buttonText.text = isObjectEnabled ? "Disable Object" : "Enable Object";
    }
}
