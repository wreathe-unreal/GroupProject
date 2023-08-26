using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : InteractableObject
{
    public GameObject textObject; // Reference to the text object
    public GameObject targetText;
    public GameObject parent;
    public TextMeshProUGUI text;
    public ActionPrompt prompt;

    public int counter = 0;
    public string keyName;
    public EDoorName AssociatedDoor;
    // Start is called before the first frame update
    void Start()
    {
        targetText = GameObject.Find("UI/Popup");
        prompt = targetText.GetComponent<ActionPrompt>();
        text = textObject.GetComponent<TextMeshProUGUI>();
        textObject.SetActive(false);
        parent = gameObject.transform.parent.gameObject;
        keyName = parent.gameObject.name;
        text.text = text.text + keyName;
    }

    public void ShowText()
    {
        if (counter == 0)
            counter++;
        prompt.activate(text.text);
    }

    public void HideText()
    {
        if (counter > 0)
            counter = 0;
        prompt.deactivate();
    }
    
    public override void Interact(PlayerCharacter player)
    {
        player.Keys.Add(AssociatedDoor);
        HideText();
        Destroy(gameObject);
    }
}
