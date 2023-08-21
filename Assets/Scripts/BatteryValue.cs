using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryValue : MonoBehaviour
{
    public TextMeshProUGUI numText;
    public Image flashlight;
    public bool isActive = false;
    public GameObject flash;
    public float battery;
    public float fps = 30.0f;

    void Start()
    {
        flash = GameObject.Find("Player/Camera/FlashlightModel/Flashlight");
        battery = flashlight.fillAmount;
        SetNumberText(battery * 100);
    }

    void Update()
    {
        updateHUD();
    }

    public void SetNumberText(float value)
    {
        numText.text = value.ToString("F1");
    }

    public void updateHUD()
    {
        if (flash.activeInHierarchy == true && battery <= 0f) flash.SetActive(false);
        else if (flash.activeInHierarchy == true && battery > 0f) battery -= 0.005f * Time.deltaTime;
        if (battery < 0f) battery = 0f;
        gameObject.GetComponent<Image>().fillAmount = battery;
        SetNumberText(battery * 100);
    }
}
