using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CosmeticsController : MonoBehaviour
{
    public Button myButton;
    private string chosenCosmetic; // Store the chosen cosmetic (or other data)

    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonPress);
        }
        else
        {
            Debug.LogError("Button is not assigned!");
        }
    }

    public void OnButtonPress()
    {
        // Store the cosmetic choice when the button is pressed
        chosenCosmetic = "Hat"; // Example: Set chosen item to "Hat"
        Debug.Log("Cosmetic chosen: " + chosenCosmetic);

        // Join a random Photon room
        PhotonNetwork.JoinRandomRoom();
    }
}
