using UnityEngine;
using TMPro;

public class Key : InteractableObject
{
    public GameObject textObject;   // Reference to the text object
    public GameObject targetText;   // Reference to HUD text
    public GameObject parent;       // Get object parent to get name of key
    public TextMeshProUGUI text;    // Key's text data
    public ActionPrompt prompt;     // Script to update UI popup text prompts

    public string keyName;          // Parent object name
    public EDoorName AssociatedDoor;// Door enum identifier
    // Start is called before the first frame update
    void Start()
    {
        targetText = GameObject.Find("UI/Popup");
        prompt = targetText.GetComponent<ActionPrompt>();
        text = textObject.GetComponent<TextMeshProUGUI>();
        textObject.SetActive(false);        // Disable text object so it doesn't stay above the key
        parent = gameObject.transform.parent.gameObject;
        keyName = parent.gameObject.name;  
        text.text = text.text + keyName;    // Set proper text with correct key name
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
        player.Keys.Add(AssociatedDoor);    // Give player ability to open dedicated door
        HideText();                         // Trigger function to empty UI popup
        Destroy(gameObject);                // Destroy GameObject as it is no longer needed
    }
}
