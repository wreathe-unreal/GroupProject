using System.Collections;
using TMPro;
using UnityEngine;

public class SpiritSpeaker : MonoBehaviour
{
    // for UI spirit line to be displayed
    public static SpiritSpeaker instance;
    public TextMeshProUGUI transcript;
    public Spirit spirit;

    private Coroutine displayTimer;

    public bool hasTextDisplayed;

    private string combinedText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTextDisplayed)
        {
            // play for one time
            hasTextDisplayed = true;
            LoadSpirit(spirit);
        }
    }

    private void Awake()
    {
        instance = this;
        spirit = GetComponent<Spirit>();
    }



    // load info from Spirit class
    public void LoadSpirit(Spirit spirit)
    {
        if (displayTimer != null)
        {
            StopCoroutine(displayTimer);
        }

        combinedText = $"{spirit.name} ({spirit.role}): {spirit.transcripts}";
        transcript.text = combinedText;

        displayTimer = StartCoroutine(TextDisplayTimer());
    }

    // start timer for text display
    private IEnumerator TextDisplayTimer()
    {
        yield return new WaitForSeconds(3f);
        transcript.text = "";
    }
}
