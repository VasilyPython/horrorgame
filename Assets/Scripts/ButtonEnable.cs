using UnityEngine;

public class ButtonEnable : MonoBehaviour
{
    public GameObject objectToEnable;
    public GameObject objectToDisable;

    // Start with the object disabled
    private bool isObjectEnabled = false;

    private void Start()
    {
        // Set the initial state of the objects based on the value of isObjectEnabled
        if (objectToEnable != null)
            objectToEnable.SetActive(isObjectEnabled);

        if (objectToDisable != null)
            objectToDisable.SetActive(!isObjectEnabled);
    }

    public void ToggleObjects()
    {
        // Toggle the enable/disable state of objects
        isObjectEnabled = !isObjectEnabled;

        if (objectToEnable != null)
            objectToEnable.SetActive(isObjectEnabled);

        if (objectToDisable != null)
            objectToDisable.SetActive(!isObjectEnabled);
    }
}
