using UnityEngine;
using UnityEngine.UI;

public class testbutton : MonoBehaviour
{
    public Button myButton; // Reference to the button

    void Start()
    {
        // Check if button is assigned and add listener
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonPress); // Use listener in code
        }
        else
        {
            Debug.LogError("Button not assigned in the inspector!");
        }
    }

    // Make sure this is a public method
    public void OnButtonPress()
    {
        Debug.Log("Button was pressed!");
    }
}
