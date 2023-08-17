using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
[AddComponentMenu("Control Script/FPS Input")]  // add to the Unity editor's component menu
public class PlayerController : MonoBehaviour
{
    public bool bAcquiredFlashlight = false;
    public bool bIsJumping = false;
    private bool isWalking = false;

    public float JumpTime { get; set; }

    // movement sensitivity
    public float speed = 6.0f;

    // gravity setting
    public float gravity = -9.8f;
    public GameObject Player;

    // reference to the character controller
    private CharacterController charController;

    //footstep
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Interact(GameObject go) 
    {
        
    }
    //attempts to interact (add Key to player inventory 
    //if the GameObject has a key tag or call OpenDoor() if the object has 
    //a Door tag by first getting the EDoorName and looping the player inventory to find a 
    //match by EDoorName 

    
    void GameOver() //displays what it needs to and sends player to main menu
    {
        
    }

    void FlashlightToggle() //responds to f key in Update() and turns on the flashlight light on the player object

    {
        
    }

    void AddInventory(GameObject key) //pressing e in Update() calls this and adds a gameobject to our player inventory

    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        float deltaY = gravity;
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        //footstep

        // changes based on WASD keys
        
        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            JumpTime = .55f;
        }

        if (JumpTime > 0)
        {
            deltaY += 15f;
            JumpTime = JumpTime - Time.deltaTime;
        }
        
        if(Input.GetButtonDown("Flashlight"))
        {
            GameObject Flashlight = transform.Find("Main Camera/Flashlight").gameObject;

            if (Flashlight.activeInHierarchy)
            {
                Flashlight.SetActive(false);
            }
            else
            {
                Flashlight.SetActive(true);
            }
        }
        
        Vector3 movement = new Vector3(deltaX, deltaY, deltaZ);

        // make diagonal movement consistent
        movement = Vector3.ClampMagnitude(movement, speed);

        // ensure movement is independent of the framerate
        movement *= Time.deltaTime;

        // transform from local space to global space
        movement = transform.TransformDirection(movement);

        // pass the movement to the character controller
        charController.Move(movement);

        HandleFootsteps(deltaX, deltaZ);
    }

    void HandleFootsteps(float deltaX, float deltaZ)
    {
        if (charController.isGrounded && (deltaX != 0 || deltaZ != 0) && !isWalking)
        {
            audioSource.Play();
            isWalking = true;
        }
        else if ((!charController.isGrounded || (deltaX == 0 && deltaZ == 0)) && isWalking)
        {
            audioSource.Stop();
            isWalking = false;
        }
    }
}
