using UnityEngine;
using UnityEngine.Video;

public class DieScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign the VideoPlayer in the Inspector

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathScreen"))
        {
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }
    }
}
