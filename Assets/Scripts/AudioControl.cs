using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource[] audioSources;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].Play();
    }

    public void PlayBGM()
    {
        if (!audioSources[0].isPlaying)
        {
            audioSources[0].Play();
        }
        if (!audioSources[1].isPlaying)
        {
            audioSources[1].Play();
        }
    }

    public void StopBGM()
    {
        if (audioSources[0].isPlaying)
        {
            audioSources[0].Stop();
        }
        if (audioSources[1].isPlaying)
        {
            audioSources[1].Stop();
        }
    }

    public void SetVolume(float volume)
    {
        audioSources[0].volume *= volume;
        audioSources[1].volume *= volume;
    }
}