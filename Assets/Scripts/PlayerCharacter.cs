using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private GameObject OverlapDoor;
    private GameObject OverlapKey;
    private PlayerController Controller;

    public double FlashlightDrainSpeed = -1.0;
    public bool bFlashlightAcquired = true;
    public bool bFlashlightActive = true;
    public List<EDoorName> Keys;
    public double FlashlightBattery = 100.0;
    public AudioSource FlashlightAudioSource;
    public InteractableObject InteractObject;

    // Start is called before the first frame update
    void Start()
    {
        var surface = GameObject.FindObjectOfType<NavMeshSurface>();
     
        surface.UpdateNavMesh (surface.navMeshData);

        Controller = GetComponent<PlayerController>();
        AudioSource flashlightAudioSource = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        ModifyFlashlightBattery(FlashlightDrainSpeed);
    }

    void ModifyFlashlightBattery(double value)
    {
            if (bFlashlightActive) 
            {
                FlashlightBattery = Math.Clamp(FlashlightBattery + (value * Time.deltaTime), 0, 100);
            }

            if (FlashlightBattery == 0 && bFlashlightActive)
            {
                ToggleFlashlight();
            }
    }

    void FindAndDisableLightByString(string path)
    {
        Transform flashlightTransform = transform.Find(path);
        if (flashlightTransform != null)
        {
            Light flashlightLight = flashlightTransform.GetComponent<Light>();
            if (flashlightLight != null)
            {
                flashlightLight.enabled = !flashlightLight.enabled;
                Debug.Log("Toggled flashlight at path: " + path);
            }
            else
            {
                Debug.LogError("Light component not found at path: " + path);
            }
        }
        else
        {
            Debug.LogError("Flashlight not found at path: " + path);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        HandleInteractableCollision(collision);
    }


    private void OnTriggerExit(Collider other)
    {
        HandleInteractableCollision(other, true);
        InteractObject = null;
    }

    private void HandleInteractableCollision(Collider collision, bool bTriggerExit = false)
    {
        InteractObject = collision.gameObject.GetComponent<InteractableObject>();
        if (InteractObject == null)
        {
            return;
        }
        
        // Project the key's world position to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(InteractObject.transform.position);

        // Determine the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Get required components
        DoorController doorScript = InteractObject.GetComponent<DoorController>();
        Key keyScript = InteractObject.GetComponent<Key>();

        float allowedScreenDeviation = 150.0f;
        // Check if the key is within the allowed deviation from the center of the screen
        if (Vector3.Distance(screenPosition, screenCenter) <= allowedScreenDeviation)
        {
            if (doorScript != null && Keys.Contains(doorScript.DoorName))
            {
                doorScript.ShowText();
            }
            if (keyScript != null)
            {
                keyScript.ShowText();
            }
        }
        else if (Vector3.Distance(screenPosition, screenCenter) > allowedScreenDeviation)
        {

            if (doorScript != null && Keys.Contains(doorScript.DoorName))
            {
                doorScript.HideText();
            }
            if (keyScript != null)
            {
                keyScript.HideText();
            }
        }
    }
        
    public void ToggleFlashlight() //responds to f key in Update() and turns on the flashlight light on the player object
    {
        if (bFlashlightAcquired) 
        {
            //set booleans
            if (bFlashlightActive)
            {
                FlashlightAudioSource.Play();
                FindAndDisableLightByString("Camera/FlashlightModel/Flashlight");
                FindAndDisableLightByString("Camera/FlashlightModel/Flashlight2");
                FindAndDisableLightByString("Camera/FlashlightModel/Flashlight3");
                FindAndDisableLightByString("Camera/FlashlightModel/ShortFlashlight");
                bFlashlightActive = false;
            }
            else
            {
                if (FlashlightBattery > 0)
                {
                    FlashlightAudioSource.Play();
                    FindAndDisableLightByString("Camera/FlashlightModel/Flashlight");
                    FindAndDisableLightByString("Camera/FlashlightModel/Flashlight2");
                    FindAndDisableLightByString("Camera/FlashlightModel/Flashlight3");
                    FindAndDisableLightByString("Camera/FlashlightModel/ShortFlashlight");
                    
                    bFlashlightActive = true;
                }
            }
        }
    }
}


