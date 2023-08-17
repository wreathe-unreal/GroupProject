using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public double FlashlightDrainSpeed = -1.0;
    public bool bFlashlightAcquired = true;
    public bool bFlashlightActive = true;
    public List<GameObject> Inventory;
    private GameObject OverlapDoor;
    private GameObject OverlapKey;
    public double FlashlightBattery = 100.0;
    private PlayerController Controller;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<PlayerController>();
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
    
    public void ToggleFlashlight() //responds to f key in Update() and turns on the flashlight light on the player object
    {
        if (bFlashlightAcquired) ;
        {
            //set booleans
            if (bFlashlightActive)
            {
                FindAndToggleFlashlight("Camera/Flashlight");
                FindAndToggleFlashlight("Camera/Flashlight2");
                FindAndToggleFlashlight("Camera/ShortFlashlight");
                bFlashlightActive = false;
            }
            else
            {
                if (FlashlightBattery > 0)
                {
                    FindAndToggleFlashlight("Camera/Flashlight");
                    FindAndToggleFlashlight("Camera/Flashlight2");
                    FindAndToggleFlashlight("Camera/ShortFlashlight");
                    
                    bFlashlightActive = true;
                }
            }
        }
    }

    void FindAndToggleFlashlight(string path)
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
}
