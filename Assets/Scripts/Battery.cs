using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battery : InteractableObject
{
    public GameObject textObject;
    public GameObject targetText;
    public TextMeshProUGUI text;
    public ActionPrompt prompt;

    // battery attributes
    public float batteryValue;  // one type of battery
    public string batteryType;

    // Start is called before the first frame update
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
        prompt.activate(text.text);         // Trigger function to display UI popup
    }

    public void HideText()
    {
        prompt.deactivate();                // Trigger function to empty UI popup
    }
    
    public override void Interact(PlayerCharacter player)
    {
        player.IncrementBattery(batteryValue);
        HideText();                         // Trigger function to empty UI popup
        Destroy(gameObject);                // Destroy GameObject as it is no longer needed
    }
}
