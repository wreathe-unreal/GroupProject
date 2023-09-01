using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

public class AudioControl : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private float defaultValue = 100;
    [SerializeField] private FloatEvent onValueLoaded;

    private void Awake()
    {
        onValueLoaded.Invoke(PlayerPrefs.GetFloat(key, defaultValue));
    }
}