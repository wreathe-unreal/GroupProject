using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InteractableObject
{
    public GameObject textObject; // Reference to the text object
    public float displayDuration; // Duration to display the text
    public EDoorName AssociatedDoor;
    // Start is called before the first frame update
    void Start()
    {
    textObject.SetActive(false);
    displayDuration = .1f; // Duration to display the text

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText()
    {
        textObject.SetActive(true);
    }

    public void HideText()
    {
        textObject.SetActive(false);
    }
    
    public override void Interact(PlayerCharacter player)
    {
        player.Keys.Add(AssociatedDoor);
        HideText();
        gameObject.SetActive(false);
    }


}
