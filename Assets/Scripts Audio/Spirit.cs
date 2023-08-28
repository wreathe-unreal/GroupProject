using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    // the script that said something about the dead spirit (principal, etc)
    public string nameSpirit;
    public string role;

    // audio part
    public AudioClip audioClips;
    public string transcripts;

    private int randomClip;    

    void Start()
    {
        audioClips = GetComponent<AudioClip>();
    }

    public void PlayDialog()
    {
        if (audioClips != null)
        {
            AudioClip clip = audioClips;
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

}
