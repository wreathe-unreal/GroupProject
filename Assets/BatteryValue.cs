using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryValue : MonoBehaviour
{
    public TextMeshProUGUI numText;
    public Image flashlight;

    void Start()
    {
        SetNumberText(flashlight.fillAmount * 100);
    }

    void Update()
    {
        SetNumberText(flashlight.fillAmount * 100);
    }

    public void SetNumberText(float value)
    {
        numText.text = value.ToString("F1");
    }
}
