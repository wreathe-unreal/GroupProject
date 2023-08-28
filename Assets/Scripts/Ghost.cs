using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.XR;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Ghost : MonoBehaviour
{
    public float GHOST_ALPHA_MAX = .7f;
    public float ghost_fast_decay = .75f;
    public float ghost_mid_decay = .4f;
    public float ghost_slow_decay = .04f;
    public float ghost_fast_return = .75f;
    public float ghost_mid_return = .4f;
    public float ghost_slow_return = .04f;
    public Transform[] waypoints;
    public NavMeshAgent agent;
    public GameObject player;
    public Animator ghostAnimate;
    public LayerMask obstructionMask;
    
    
    
    private int currentPoint = 0;
    private Vector3 target;
    private Vector3 direction;
    //FieldOfView FOV;
    private bool bSpotted = false;
    private Camera playerCamera;
    private bool inPursuit = false;
    private float pursuitTime = 0;
    private int pursuitDur = 30;
    private bool reachStart = false;
    private int prevPoint =-1;
    private Light ghostGlow;
    private GhostFOV ghostFOV;
    private MeshRenderer ghostMesh;
    private Color ghostMaterialColor;
    private Material ghostMaterial;
    private bool bGhostRespawnTimerFinished = true;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ghostMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        ghostMaterial = ghostMesh.material; 
        ghostMaterialColor = ghostMaterial.color;
        ghostMaterialColor.a = GHOST_ALPHA_MAX;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        ghostAnimate = GetComponent<Animator>();
        ghostGlow = GetComponent<Light>();
        ghostFOV = transform.GetChild(1).GetComponent<GhostFOV>();
        ghostMaterialColor = ghostMaterial.color;
        ghostMaterial.SetFloat("_Mode", 3); // Set to Transparent mode
        ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        ghostMaterial.SetInt("_ZWrite", 0);
        ghostMaterial.DisableKeyword("_ALPHATEST_ON");
        ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
        ghostMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        ghostMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGhostBehavior();
    }

    private void UpdateGhostBehavior ()
    {
        Vector3 PlayerTransformAdjusted = player.transform.position;
        PlayerTransformAdjusted.y = transform.position.y; //set Player Transform Y to Ghosts's for distance calculation
        
        float dist = Vector3.Distance(PlayerTransformAdjusted, transform.position);

        bSpotted = ghostFOV.bSpotted;
        
        ReactToLight();
        
        //Spotted target
        if ((bSpotted || dist < 4f) && bGhostRespawnTimerFinished)
        {
            SpottedAnimation();
            ghostGlow.color = Color.red;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            agent.destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

            inPursuit = true;
            
            if (dist < .75f && ghostMaterialColor.a > .1)
            {
                CatchPlayer();
            }

        }
        //lost sight of target // hunting mode
        else if (bSpotted == false && inPursuit && pursuitTime < pursuitDur && bGhostRespawnTimerFinished)
        {
            //ghostAnimate.SetTrigger("searching");
            ghostGlow.color = Color.yellow;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            agent.destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            pursuitTime += Time.deltaTime;
            
            if (pursuitTime >= pursuitDur)
            {
                inPursuit = false;
                pursuitTime = 0;
            }
        }
        //Patrol state
        else
        {
            //ghostAnimate.SetTrigger("searching");
            ghostGlow.color = Color.white;
            if (waypoints.Length > 0)
            {
                target = new Vector3(waypoints[currentPoint].position.x, transform.position.y, waypoints[currentPoint].position.z);
                transform.LookAt(target); 
            }
            
            direction = target - transform.position;
            direction.y = transform.position.y;

            if (direction.magnitude < 1.5 && reachStart == false)
            {
                reachStart = true;
                ChooseBehavior();
                //currentPoint = Random.Range(0, waypoints.Length);
            }
            
            agent.destination = target;
            
            inPursuit = false;
        }
    }

    void CatchPlayer()
    {
        if (inPursuit && ghostMaterialColor.a > .05)
        {
            //Insert Jumpscare etc
            ghostGlow.color = Color.green;
        }
    }

    //Function for random behavior when ghost is at specific waypoint
    void ChooseBehavior()
    {
        int rando = Random.Range(0, 3);
        //int rando = 2;
        switch (rando)
        {
            case 0:         //straight Next poimt
                Invoke("RandomWaypoint", 1.0f);
                break;
            case 1:         //idle
                //Invoke("investigate", 1.0f);
                Invoke("RandomWaypoint", 2.0f);
                break;
            case 2:         //Looking around
                LookAnimation();
                Invoke("RandomWaypoint", 4.0f);
                Invoke("SearchAnimation", 4.0f);
                break;
        }
    }

    void SpottedAnimation()
    {
        ghostAnimate.SetTrigger("spotted");
    }
    void SearchAnimation()
    {
        ghostAnimate.SetTrigger("searching");
    }
    void LookAnimation()
    {
        ghostAnimate.SetTrigger("looking");
    }

    //Set next waypoint for ghost to patrol
    int RandomWaypoint()
    {
        prevPoint = currentPoint; 
        currentPoint = Random.Range(0, waypoints.Length);
        while (currentPoint == prevPoint)
        {
            currentPoint = Random.Range(0, waypoints.Length);
        }
        reachStart = false;
        return currentPoint;
    }

    void ReactToLight()
    {

        Vector3 screenPosition = playerCamera.WorldToScreenPoint(transform.position + new Vector3(0f, 1.6f, 0f)); //increase the ghost's origin 
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        // Define allowed screen deviation from the center
        float allowedScreenDeviation = 250.0f;

// Compute distance from the screen center to the ghost's projected position
        float distanceFromCenter = Vector3.Distance(screenPosition, screenCenter);
        
        if (distanceFromCenter <= allowedScreenDeviation)
        {
            // Raycast to check for obstruction
            float distanceToGhost = Vector3.Distance(transform.position, player.transform.position);
        
            if (!Physics.Raycast(player.transform.position, player.transform.forward, distanceToGhost, obstructionMask))
            {
                if (player.GetComponent<PlayerCharacter>().bFlashlightActive)
                {
                    //Debug.Log("Player is facing the ghost and not obstructed.");
                    BanishGhost();
                } 
            }
            else
            {
                // Obstruction detected
                //Debug.Log("Player is facing the ghost, but there is an obstruction.");
                ReviveGhost();
            }
        }
        else
        {
            // Player is not facing the ghost
            //Debug.Log("Player is not facing the ghost.");
            ReviveGhost();
        }
       
    }
    
    

    // Function to call when you want to start the respawn timer
    public void StartGhostRespawnTimer()
    {
        StartCoroutine(GhostRespawnTimer());
    }
    // Coroutine for the 4-second timer
    IEnumerator GhostRespawnTimer()
    {
        bGhostRespawnTimerFinished = false; // Reset the flag
        yield return new WaitForSeconds(6.0f); // Wait for 4 seconds
        bGhostRespawnTimerFinished = true; // Set the flag to true
    }
    
    void ReviveGhost()
    {
        if (bGhostRespawnTimerFinished)
        {
            // Obstruction detected
            //Debug.Log("Player is facing the ghost, but there is an obstruction.");
            if (ghostMaterialColor.a > .2f)
            {
                ghostMaterialColor.a += Time.deltaTime * ghost_fast_return;
            }

            if (ghostMaterialColor.a <= .2f && ghostMaterialColor.a > .1f)
            {
                ghostMaterialColor.a += Time.deltaTime * ghost_mid_return;
            }

            if (ghostMaterialColor.a <= .1f)
            {
                ghostMaterialColor.a += Time.deltaTime * ghost_slow_return;
            }

            if (ghostMaterialColor.a > GHOST_ALPHA_MAX)
            {
                ghostMaterialColor.a = GHOST_ALPHA_MAX;
            }

            ghostMaterial.color = ghostMaterialColor;
        }
    }

    void BanishGhost()
    {
        // Adjust the alpha value
        if (ghostMaterialColor.a > .2f)
        {
            ghostMaterialColor.a -= Time.deltaTime * ghost_fast_decay;
        }

        else if (ghostMaterialColor.a <= .2f && ghostMaterialColor.a > .1f)
        {
            ghostMaterialColor.a -= Time.deltaTime* ghost_mid_decay;
        }

        else if (ghostMaterialColor.a <= .1f)
        {
            ghostMaterialColor.a -= Time.deltaTime * ghost_slow_decay;
        }

        if (ghostMaterialColor.a < .05 && bGhostRespawnTimerFinished)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();   
            }
        }
        if (ghostMaterialColor.a < 0f)
        {
            ghostMaterialColor.a = 0f;
            StartGhostRespawnTimer();
        }
        ghostMaterial.color = ghostMaterialColor;

        // Set the ghost into patrol mode
        bSpotted = false;
    }
    
    
    
}
