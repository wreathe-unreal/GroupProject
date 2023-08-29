using System.Collections;
using TMPro;
using UnityEngine;

public class SpiritSpeaker : MonoBehaviour
{
    // for UI spirit line to be displayed
    public static SpiritSpeaker instance;
    public Spirit spirit;

    private Coroutine displayTimer;
    private TextMeshProUGUI script;

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
        script = GameObject.Find("UI/Popup 2").GetComponent<TextMeshProUGUI>();
    }


    // load info from Spirit class
    public void LoadSpirit(Spirit spirit)
    {
        if (displayTimer != null)
        {
            StopCoroutine(displayTimer);
        }

        combinedText = $"{spirit.name}: {spirit.transcript}";
        script.text = combinedText;

        displayTimer = StartCoroutine(TextDisplayTimer());
    }

    // start timer for text display
    private IEnumerator TextDisplayTimer()
    {
        yield return new WaitForSeconds(3f);
        script.text = "";
    }
}
