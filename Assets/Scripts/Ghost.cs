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

    public Transform[] waypoints;
    private int currentPoint = 0;
    private Vector3 target;
    private Vector3 direction;

    NavMeshAgent agent;
    GameObject player;

    //FieldOfView FOV;
    bool bSpotted = false;
    private Camera playerCamera;
    private bool inPursuit = false;
    private float pursuitTime = 0;
    [SerializeField]
    private int pursuitDur = 30;

    private bool bGhostAggressive = false;

    Animator ghostAnimate;

    private bool onetime = false;
    private bool reachStart = false;

    private int prevPoint =-1;

    private Light ghostGlow;
    private GhostFOV ghostFOV;
    public LayerMask obstructionMask;
    private MeshRenderer ghostMesh;
    private Color ghostMaterialColor;
    private Material ghostMaterial;
    private bool bGhostRespawnTimerFinished = true;
    private AudioSource[] audioSources;
    private bool bRecentlyRespawned = false;
    private float RecentlyRespawnedTimer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ghostMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        ghostMaterial = ghostMesh.material; 
        ghostMaterialColor = ghostMaterial.color;
        ghostMaterialColor.a = .65f;
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
        Debug.Log(ghostMaterialColor.a);
    }

    private void UpdateGhostBehavior ()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        bSpotted = ghostFOV.bSpotted;
        
        ReactToLight();
        
        //Spotted target
        if (bSpotted || dist < 1.5f)
        {
            ghostAnimate.SetTrigger("spotted");
            ghostGlow.color = Color.red;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            agent.destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

            inPursuit = true;
            
            if (dist < 1.45f)
            {
                CatchPlayer();
            }

        }
        //lost sight of target // hunting mode
        else if (bSpotted == false && inPursuit && pursuitTime < pursuitDur)
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
        if (!onetime && inPursuit)
        {
            //Insert Jumpscare etc

            onetime = true;
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
        float allowedScreenDeviation = 300.0f;

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
                    
                    // Adjust the alpha value
                    if (ghostMaterialColor.a > .2f)
                    {
                        ghostMaterialColor.a -= Time.deltaTime * 8f;
                    }

                    if (ghostMaterialColor.a <= .2f && ghostMaterialColor.a > .1f)
                    {
                        ghostMaterialColor.a -= Time.deltaTime * 5;
                    }

                    if (ghostMaterialColor.a <= .1f)
                    {
                        ghostMaterialColor.a -= Time.deltaTime * .03f;
                    }
                    if (ghostMaterialColor.a < 0f)
                    {
                        ghostMaterialColor.a = 0f;
                        bGhostRespawnTimerFinished = false;
                        if (!bRecentlyRespawned)
                        {
                            StartGhostRespawnTimer();
                            RecentlyRespawnedTimer += 5.0f;
                        }
                    }
                    ghostMaterial.color = ghostMaterialColor;

                    // Set the ghost into patrol mode
                    bSpotted = false;
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
        yield return new WaitForSeconds(4.0f); // Wait for 4 seconds
        bGhostRespawnTimerFinished = true; // Set the flag to true
        bRecentlyRespawned = true;
    }
    
    void ReviveGhost()
    {
        if (bGhostRespawnTimerFinished)
        {
            // Obstruction detected
            //Debug.Log("Player is facing the ghost, but there is an obstruction.");
            if (ghostMaterialColor.a > .2f)
            {
                ghostMaterialColor.a += Time.deltaTime * 12f;
            }

            if (ghostMaterialColor.a <= .2f && ghostMaterialColor.a > .1f)
            {
                ghostMaterialColor.a += Time.deltaTime * 6;
            }

            if (ghostMaterialColor.a <= .1f)
            {
                ghostMaterialColor.a += Time.deltaTime * .04f;
            }

            if (ghostMaterialColor.a > .65f)
            {
                ghostMaterialColor.a = .65f;
            }

            ghostMaterial.color = ghostMaterialColor;
        }
    }

    void bRecentlyRespawnedTimer(bool bRecentlyRespawned)
    {
        if (RecentlyRespawnedTimer <= 0)
        {
            bRecentlyRespawned = false;
        }
        if (bRecentlyRespawned)
        {
            RecentlyRespawnedTimer -= Time.deltaTime;
        }
    }
    
    
    
}
