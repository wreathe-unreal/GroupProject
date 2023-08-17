using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
[AddComponentMenu("Control Script/FPS Input")]  // add to the Unity editor's component menu
public class FPSInput : MonoBehaviour
{
    public bool bIsJumping = false;
    private bool isWalking = false;

    public float JumpTime { get; set; }

    // movement sensitivity
    public float speed = 6.0f;

    // gravity setting
    public float gravity = -9.8f;

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
