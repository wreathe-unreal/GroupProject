using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

public enum EDoorName
{
    EscapeDoor = 0,
    ProjectorRoom = 1,
    ClassRoom = 2,
    PrincipalRoom = 3,
    FarClassRoom = 4,
    Cafeteria =5
}

public class DoorController : InteractableObject
{
    public EDoorName DoorName;
    // Start is called before the first frame update
    public GameObject textObject; // Reference to the text object
    public GameObject targetText;
    public TextMeshProUGUI text;
    public ActionPrompt prompt;
    public int counter = 0;

    public Animator AnimationController;
    public AudioSource LockedDoorAudio;
    public AudioSource OpenDoorAudio;

    private bool bDoorIsClosed = true;
    
    
    void Start()
    {
        targetText = GameObject.Find("UI/Popup");
        prompt = targetText.GetComponent<ActionPrompt>();
        text = textObject.GetComponent<TextMeshProUGUI>();
        textObject.SetActive(false);
    }

    public void ShowText()
    {
        if (bDoorIsClosed)
        {
            if (counter == 0)
                counter++;
            prompt.activate(text.text);
        }
    }

    public void HideText()
    {
        if (counter > 0)
            counter = 0;
        prompt.deactivate();
    }

    public void OpenDoor()
    {
        AnimationController.SetTrigger("OpenDoor");
    }
    
    public override void Interact(PlayerCharacter player)
    {
        if (bDoorIsClosed)
        {
            if (player.Keys.Contains(DoorName))
            {
                OpenDoorAudio.Play();
                OpenDoor();
                HideText();
                bDoorIsClosed = false;
            }
            else
            {
                LockedDoorAudio.Play();    
            }    
        }
    }
}
