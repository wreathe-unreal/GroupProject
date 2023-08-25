using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
[AddComponentMenu("Control Script/FPS Input")]  // add to the Unity editor's component menu
public class PlayerController : MonoBehaviour
{
    public bool bIsJumping = false;
    private bool bIsWalking = false;

    public float JumpTime { get; set; }

    // movement sensitivity
    public float speed = 6.0f;

    // gravity setting
    public float gravity = -9.8f;

    // reference to the character controller
    private PlayerCharacter Player;
    private CharacterController CharController;

    //footstep
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Player = GetComponent<PlayerCharacter>();
        CharController = GetComponent<CharacterController>();
    }

    void Interact(InteractableObject io)
    {
        //pass the interaction to the interactable object
        if (io != null)
        {
            io.Interact(Player); 
        }
    }

    
    void GameOver() //displays what it needs to and sends player to main menu
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
        if(Input.GetButtonDown("Flashlight"))
        {

            Player.ToggleFlashlight();
        }
        //footstep
        if(Input.GetButtonDown("Interact"))
        {

            Interact(Player.InteractObject);
        }
        
        Vector3 movement = new Vector3(deltaX, deltaY, deltaZ);

        // make diagonal movement consistent
        movement = Vector3.ClampMagnitude(movement, speed);

        // ensure movement is independent of the framerate
        movement *= Time.deltaTime;

        // transform from local space to global space
        movement = transform.TransformDirection(movement);

        // pass the movement to the character controller
        CharController.Move(movement);

        HandleFootsteps(deltaX, deltaZ);
    }

    void HandleFootsteps(float deltaX, float deltaZ)
    {
        if (CharController.isGrounded && (deltaX != 0 || deltaZ != 0) && !bIsWalking)
        {
            audioSource.Play();
            bIsWalking = true;
        }
        else if ((!CharController.isGrounded || (deltaX == 0 && deltaZ == 0)) && bIsWalking)
        {
            audioSource.Stop();
            bIsWalking = false;
        }
    }
    
}
