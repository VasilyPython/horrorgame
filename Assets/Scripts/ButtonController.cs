using Photon.Pun;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject cosmeticCube; // Assign your cube here via the Inspector
    private static bool shouldActivateCube = false; // Static variable to track activation state

    private void Start()
    {
        // Optionally, ensure the cube is inactive at the start
        if (cosmeticCube != null)
        {
            cosmeticCube.SetActive(false);
        }
    }

    public void OnButtonPress()
    {
        // Check if connected to Photon and in a room
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Not in a room! Storing activation state.");
            shouldActivateCube = true; // Store the state instead of calling RPC
            return; // Exit early if not in a room
        }

        // Get the local player's GameObject (make sure you have a way to reference it)
        GameObject localPlayerObject = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        if (localPlayerObject != null)
        {
            // Get the PhotonView from the local player GameObject
            PhotonView photonView = localPlayerObject.GetComponent<PhotonView>();
            
            if (photonView != null)
            {
                // Call the RPC to activate the cube
                photonView.RPC("ActivateCube", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void ActivateCube()
    {
        if (cosmeticCube != null)
        {
            cosmeticCube.SetActive(true); // Activate the cube for all players
        }
    }

    public static void ActivateStoredCube()
    {
        shouldActivateCube = false; // Reset the state
    }

    public static bool ShouldActivateCube()
    {
        return shouldActivateCube; // Getter for the state
    }
}
