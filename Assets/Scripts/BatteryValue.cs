using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryValue : MonoBehaviour
{
    public TextMeshProUGUI numText;
    public Image flashlight;
    public bool isActive = false;
    public GameObject player;
    public float battery;
    public float fps = 30.0f;

    void Start()
    {
        player = GameObject.Find("Player");
        SetNumberText(battery * 100);
    }

    void Update()
    {
        isActive = player.GetComponent<PlayerCharacter>().bFlashlightActive;
        battery = (float) player.GetComponent<PlayerCharacter>().FlashlightBattery;
        updateHUD();
    }

    public void SetNumberText(float value)
    {
        numText.text = value.ToString("F1");
    }

    public void updateHUD()
    {
        gameObject.GetComponent<Image>().fillAmount = (battery / 100);
        SetNumberText(battery);
    }
}
