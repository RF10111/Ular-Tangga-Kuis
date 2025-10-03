using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;

    private void PlayBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void StopBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}