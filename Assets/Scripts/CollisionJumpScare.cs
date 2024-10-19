using UnityEngine;
using System.Collections;

public class CollisionJumpScare : MonoBehaviour
{
    public GameObject scareObject;  // Assign the GameObject you want to activate in the Inspector
    public AudioSource scareSound;  // Assign the AudioSource component in the Inspector
    private bool isOnCooldown = false;  // Tracks if the sound is on cooldown

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dingal") && !isOnCooldown)
        {
            // Activate the scareObject
            scareObject.SetActive(true);

            // Play the sound immediately
            if (scareSound != null)
            {
                scareSound.Play();
            }

            // Start cooldown to prevent sound from playing again too soon
            StartCoroutine(SoundCooldown(15f));  // 15-second cooldown
        }
    }

    // Coroutine to handle the 15-second cooldown
    private IEnumerator SoundCooldown(float cooldownTime)
    {
        isOnCooldown = true;  // Set cooldown flag
        yield return new WaitForSeconds(cooldownTime);  // Wait for the cooldown period
        isOnCooldown = false;  // Reset cooldown after waiting
    }
}
