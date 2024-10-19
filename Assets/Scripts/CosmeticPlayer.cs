using Photon.Pun;
using UnityEngine;

public class CosmeticPlayer : MonoBehaviourPunCallbacks
{
    public GameObject cosmeticCube; // Reference to the cosmetic cube

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Check if the cube should be activated
        if (ButtonController.ShouldActivateCube())
        {
            ActivateCosmeticCube();
            ButtonController.ActivateStoredCube(); // Reset the state after activation
        }
    }

    private void ActivateCosmeticCube()
    {
        if (cosmeticCube != null)
        {
            cosmeticCube.SetActive(true); // Activate the cube for this player
        }
    }
}
