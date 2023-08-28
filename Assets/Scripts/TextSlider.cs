using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
    [SerializeField] private string key;

    public TextMeshProUGUI numText;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        SetNumberText(slider.value);
    }

    public void SetNumberText(float value)
    {
        numText.text = (value * 100).ToString("F0");
        PlayerPrefs.SetFloat(key, value);
    }
}
