using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

public class AudioControl : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private float defaultValue = 0;
    [SerializeField] private FloatEvent onValueLoaded;

    public AudioSource[] audioSources;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].Play();
    }

    private void Awake()
    {
        onValueLoaded.Invoke(PlayerPrefs.GetFloat(key, defaultValue));
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