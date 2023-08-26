using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;

public class Key : InteractableObject
{
    private int counter = 0;

    public GameObject textObject; // Reference to the text object
    public GameObject targetText;
    public GameObject parent;
    public TextMeshProUGUI text;
    public ActionPrompt prompt;

    public float displayDuration; // Duration to display the text
    public string keyName;

    public EDoorName AssociatedDoor;

    // Start is called before the first frame update
    void Start()
    {
        targetText = GameObject.Find("UI/Popup");
        prompt = targetText.GetComponent<ActionPrompt>();
        text = textObject.GetComponent<TextMeshProUGUI>();
        textObject.SetActive(false);
        keyName = parent.gameObject.name;
        text.text = text.text + " " + keyName;
        displayDuration = .1f; // Duration to display the text
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText()
    {
        if (counter == 0)
        {
            counter++;
        }
        prompt.activate(text.text.ToString());
    }

    public void HideText()
    {
        if (counter > 0)
        {
            counter--;
        }
        prompt.deactivate();
    }
    
    public override void Interact(PlayerCharacter player)
    {
        player.Keys.Add(AssociatedDoor);
        HideText();
        Destroy(gameObject);
    }
}
