using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip soundClip; // The sound clip to be played
    public float minTime = 5f;  // Minimum time interval between plays
    public float maxTime = 15f; // Maximum time interval between plays

    private AudioSource audioSource;
    private float nextPlayTime;

    void Start()
    {
        // Add an AudioSource component if one doesn't already exist
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
        ScheduleNextPlay();
    }

    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            PlaySound();
            ScheduleNextPlay();
        }
    }

    void PlaySound()
    {
        audioSource.Play();
    }

    void ScheduleNextPlay()
    {
        nextPlayTime = Time.time + Random.Range(minTime, maxTime);
    }
}
