using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDoorName
{
    RedDoor = 0, //or whatever you want to name it
    BlueDoor = 1,
    GreenDoor = 2
}

public class DoorController : InteractableObject
{
    public EDoorName DoorName;
    private bool bDoorIsClosed = true;
    // Start is called before the first frame update
    public GameObject textObjectFront; // Reference to the text object
    public GameObject textObjectBack; // Reference to the text object
    public float displayDuration; // Duration to display the text

    public Animator AnimationController;
    // Start is called before the first frame update
    void Start()
    {
        textObjectFront.SetActive(false);
        textObjectBack.SetActive(false);
        displayDuration = .1f; // Duration to display the text

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText()
    {
        textObjectFront.SetActive(true);
        textObjectBack.SetActive(true);

    }

    public void HideText()
    {
        textObjectFront.SetActive(false);
        textObjectBack.SetActive(false);

    }

    public void OpenDoor()
    {
        AnimationController.SetTrigger("OpenDoor");
    }
    
    public override void Interact(PlayerCharacter player)
    {
        if (player.Keys.Contains(DoorName))
        {
            OpenDoor();
            HideText();
            textObjectFront.transform.parent.gameObject.SetActive(false);
            textObjectBack.transform.parent.gameObject.SetActive(false);
            
        }
    }
}
