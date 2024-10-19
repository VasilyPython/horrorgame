using UnityEngine;
using UnityEngine.Video;

public class DeathScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Update()
    {
        // Check if the 'R' key is pressed and the video player is active
        if (Input.GetKeyDown(KeyCode.R) && videoPlayer.isPlaying)
        {
            // Stop and disable the video player
            videoPlayer.Stop();
            videoPlayer.enabled = false;
            Debug.Log("Video Player Disabled");
        }

        // Check if the 'E' key is pressed
        if (Input.GetKeyDown(KeyCode.E) && videoPlayer.isPlaying)
        {
            Debug.Log("Quit"); // Print "Quit" in the console
            Application.Quit(); // Quit the application
        }
    }
}
