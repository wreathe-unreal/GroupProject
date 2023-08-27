using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughterTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasTriggered = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;      
            audioSource.loop = true;  
            audioSource.Play();       
            Invoke("StopLaughter", 10f); 
        }
    }

    void StopLaughter()
    {
        audioSource.Stop();         // Stop the sound
    }
}
