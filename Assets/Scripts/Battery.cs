using TMPro;
using UnityEngine;

public class Battery : InteractableObject
{
    public GameObject textObject;
    public GameObject targetText;
    public TextMeshProUGUI text;
    public ActionPrompt prompt;

    // battery attributes
    public float batteryValue;  // one type of battery
    public string batteryType;

    void Start()
    {
        targetText = GameObject.Find("UI/Popup");
        prompt = targetText.GetComponent<ActionPrompt>();
        text = textObject.GetComponent<TextMeshProUGUI>();
        textObject.SetActive(false);
        batteryType = gameObject.name;
        text.text = text.text + batteryType;
    }

    public void ShowText()
    {
        prompt.activate(text.text);
    }

    public void HideText()
    {
        prompt.deactivate();
    }

    public override void Interact(PlayerCharacter player)
    {
        player.IncrementBattery(batteryValue);
        HideText();
        Destroy(gameObject);
    }
}