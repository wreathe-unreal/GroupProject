using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUpdate : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1.0f * Time.deltaTime;
        int inttime = (int)timer;
        Debug.Log(timer);
        string minutes = ((int)(timer/60)).ToString("F0");
        float seconds = (timer % 60);
        string secondss = seconds.ToString("F0");
        if (seconds != 0 ) text.text = minutes + ":" + secondss;
        else text.text = minutes + ":0" + secondss;
    }
}
