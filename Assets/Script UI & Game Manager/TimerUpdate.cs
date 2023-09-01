using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUpdate : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float timer = 0f;
    public int mintime = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1.0f * Time.deltaTime;
        if (timer >= 60f)
        {
            mintime += 1;
            timer -= 60f;
        }
        string minutes = mintime.ToString("F0");
        int sectime = (int)timer;
        string seconds = sectime.ToString("F0");
        if (sectime < 10) text.text = minutes + ":0" + seconds;
        else text.text = minutes + ":" + seconds;
    }
}
