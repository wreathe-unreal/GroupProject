using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;

public class DisplayTrigger : MonoBehaviour
{
    public Light ProjectorLight;
    public AudioSource audioSource;
    public TextMeshPro displayText;
    private bool bHasDisplayed = false;
    
    private float displayDuration = 4f;  
    private int currentTextIndex = 0;
    private Renderer planeRenderer;
   
    private string[] textSequence = {
        "Hey!",
        "You...",
        "You are not suppose to be here.",
        "RUN.",
        "Take the key, open the door, and let me out.",
        "Since you'll help me, I'll help you.",
        "Remember the light is your weapon.",
        "Fade the cursed spirits with your light...",
        "...or perish when they consume you!"
    };
    private void Start()
    {
        planeRenderer = GetComponent<Renderer>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !bHasDisplayed)
        {
            ProjectorLight.color = Color.red;
            ProjectorLight.intensity = 500;
            PlayAudioAndStartDisplayingText();
             bHasDisplayed = true;
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
        if(currentTextIndex < textSequence.Length)
        {
            displayText.text = textSequence[currentTextIndex];
            currentTextIndex++;
            Invoke("DisplayText", displayDuration);
        }
 // Choose the intensity value that suits your application
    }

}
