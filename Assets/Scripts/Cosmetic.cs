using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for using UI components

public class Cosmetic : MonoBehaviour
{
    public List<string> cosmetics; // List of cosmetic button tags to check
    public GameObject objectToEnable; // GameObject to enable when a cosmetic button is pressed

    private void Start()
    {
        // Assuming there are UI buttons set up in the scene, add listeners to them
        foreach (string tag in cosmetics)
        {
            Button button = GameObject.FindGameObjectWithTag(tag)?.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnCosmeticButtonPressed(tag));
            }
        }
    }

    private void OnCosmeticButtonPressed(string tag)
    {
        // Enable the specified GameObject
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }

    // If you still want to handle collision with a DeathScreen tag
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathScreen"))
        {
            // Handle the collision if needed (this part can be removed if not necessary)
        }
    }
}
