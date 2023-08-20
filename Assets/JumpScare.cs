using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    public Image ghostImage;
    public AudioSource scream;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            ShowGhost();
            Debug.Log("Playing scream");
            scream.Play();
            Invoke("HideGhost", 1f); 
        }
    }

    void ShowGhost()
    {
        ghostImage.color = new Color(1f, 1f, 1f, 1f);
    }

    void HideGhost()
    {
        ghostImage.color = new Color(1f, 1f, 1f, 0f); 
    }
}