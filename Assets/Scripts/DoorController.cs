using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EDoorName
{
    RedDoor, //or whatever you want to name it
    BlueDoor,
    GreenDoor
}

public class DoorController : MonoBehaviour
{
    public EDoorName DoorName;
    private bool bDoorIsClosed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
