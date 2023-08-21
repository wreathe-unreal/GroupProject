using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject textObject; // Reference to the text object
    public float displayDuration; // Duration to display the text
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


}
