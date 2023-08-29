using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
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
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.SetFloat("Voices", value);
    }
}
