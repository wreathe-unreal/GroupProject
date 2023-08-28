using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritSpeaker : MonoBehaviour
{
    // for UI spirit line to be displayed
    public static SpiritSpeaker instance;
    public Text transcript;

    private string combinedText;
    
    private void Awake()
    {
        instance = this;        
    }

    // load info from Spirit class
    public void LoadSpirit(Spirit spirit)
    {
        combinedText = $"{spirit.name} ({spirit.role}): {spirit.transcripts}";
        transcript.text = combinedText;
    }
}
