using UnityEngine;

public class CosmeticHandler : MonoBehaviour
{
    public GameObject[] allCosmetics; // Assign all possible cosmetics in the inspector
    private GameObject currentCosmetic; // Reference to the currently active cosmetic

    public void ActivateCosmetic(string cosmeticName)
    {
        // Destroy the current cosmetic if it exists
        if (currentCosmetic != null)
        {
            Destroy(currentCosmetic);
        }

        // Find and instantiate the selected cosmetic
        foreach (GameObject cosmetic in allCosmetics)
        {
            if (cosmetic.name == cosmeticName)
            {
                currentCosmetic = Instantiate(cosmetic, transform); // Instantiate as child of this handler
                currentCosmetic.SetActive(true); // Activate the instantiated cosmetic
                Debug.Log("Activated cosmetic: " + cosmeticName);
                break;
            }
        }
    }
}
