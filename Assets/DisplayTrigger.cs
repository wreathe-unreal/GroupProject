using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public TextMeshPro displayText;

   
    public string[] textSequence = {
        "You are not suppose to be here.",
        "RUN.",
        "Remember the light is the key."
    };

    public float displayDuration = 5f;  
    private int currentTextIndex = 0;
    private Renderer planeRenderer;

    private void Start()
    {
        planeRenderer = GetComponent<Renderer>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayAudioAndStartDisplayingText();
        }
    }

    void PlayAudioAndStartDisplayingText()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            DisplayText();
        }
    }

    void DisplayText()
    {
        if (currentTextIndex < textSequence.Length)
        {
            displayText.text = textSequence[currentTextIndex];
            currentTextIndex++;
            Invoke("DisplayText", displayDuration);
        }
        else
        {
            displayText.text = "";
            TurnPlaneRed();
        }
    }

    void TurnPlaneRed()
    {
        planeRenderer.material.color = Color.red; 
    }
}
