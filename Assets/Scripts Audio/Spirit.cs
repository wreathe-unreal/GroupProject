using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    // audio part
    public AudioClip audioClips;
    public string transcript;

    private string nameSpirit;
    private int randomClip;

    void Start()
    {
        nameSpirit = gameObject.name;
    }

    public void PlayDialog()
    {
        if (audioClips != null)
        {
            AudioClip clip = audioClips;
            AudioSource.PlayClipAtPoint(clip, transform.position);
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
