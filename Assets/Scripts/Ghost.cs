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
    public float MATERIAL_ALPHA_MAX = .75f;
    public float FastDecay = .8f;
    public float MidDecay = .5f;
    public float SlowDecay = .05f;
    public float FastReturn = .6f;
    public float MidReturn = .3f;
    public float SlowReturn = .03f;
    public Transform[] waypoints;
    public NavMeshAgent agent;
    public GameObject player;
    public Animator AnimationController;
    public LayerMask obstructionMask;
    public AudioSource ghostBanish;




    private JumpScare DeathScene;
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
    private Light Glow;
    private GhostFOV ghostFOV;
    private MeshRenderer Mesh;
    private Color MaterialColor;
    private Material MaterialComponent;
    private bool bRespawnTimerFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        MaterialComponent = Mesh.material; 
        MaterialColor = MaterialComponent.color;
        MaterialColor.a = MATERIAL_ALPHA_MAX;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        AnimationController = GetComponent<Animator>();
        Glow = GetComponent<Light>();
        ghostFOV = transform.GetChild(1).GetComponent<GhostFOV>();
        DeathScene = GetComponent<JumpScare>();
        MaterialColor = MaterialComponent.color;
        MaterialComponent.SetFloat("_Mode", 3); // Set to Transparent mode
        MaterialComponent.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        MaterialComponent.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        MaterialComponent.SetInt("_ZWrite", 0);
        MaterialComponent.DisableKeyword("_ALPHATEST_ON");
        MaterialComponent.EnableKeyword("_ALPHABLEND_ON");
        MaterialComponent.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        MaterialComponent.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
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
        if ((bSpotted || dist < 4f) && bRespawnTimerFinished)
        {
            SpottedAnimation();
            Glow.color = Color.red;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            agent.destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

            inPursuit = true;
            
            if (dist < .75f && MaterialColor.a > .1)
            {
                CatchPlayer();
            }

        }
        //lost sight of target // hunting mode
        else if (bSpotted == false && inPursuit && pursuitTime < pursuitDur && bRespawnTimerFinished)
        {
            //ghostAnimate.SetTrigger("searching");
            Glow.color = Color.yellow;
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
            Glow.color = Color.white;
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
        if (inPursuit && MaterialColor.a > .05)
        {
            //Insert Jumpscare etc
            Glow.color = Color.green;
            DeathScene.Trigger();
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
        AnimationController.SetTrigger("spotted");
    }
    void SearchAnimation()
    {
        AnimationController.SetTrigger("searching");
    }
    void LookAnimation()
    {
        AnimationController.SetTrigger("looking");
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
        bRespawnTimerFinished = false; // Reset the flag
        yield return new WaitForSeconds(6.0f); // Wait for 4 seconds
        bRespawnTimerFinished = true; // Set the flag to true
    }
    
    void ReviveGhost()
    {
        if (bRespawnTimerFinished)
        {
            // Obstruction detected
            //Debug.Log("Player is facing the ghost, but there is an obstruction.");
            if (MaterialColor.a > .2f)
            {
                MaterialColor.a += Time.deltaTime * FastReturn;
            }

            if (MaterialColor.a <= .2f && MaterialColor.a > .1f)
            {
                MaterialColor.a += Time.deltaTime * MidReturn;
            }

            if (MaterialColor.a <= .1f)
            {
                MaterialColor.a += Time.deltaTime * SlowReturn;
            }

            if (MaterialColor.a > MATERIAL_ALPHA_MAX)
            {
                MaterialColor.a = MATERIAL_ALPHA_MAX;
            }

            MaterialComponent.color = MaterialColor;
        }
    }

    void BanishGhost()
    {
        // Adjust the alpha value
        if (MaterialColor.a > .2f)
        {
            MaterialColor.a -= Time.deltaTime * FastDecay;
        }

        else if (MaterialColor.a <= .2f && MaterialColor.a > .1f)
        {
            MaterialColor.a -= Time.deltaTime * MidDecay;
        }

        else if (MaterialColor.a <= .1f)
        {
            MaterialColor.a -= Time.deltaTime * SlowDecay;
        }

        if (MaterialColor.a < .05f && bRespawnTimerFinished)
        {
            if (!ghostBanish.isPlaying)
            {
                Debug.Log(ghostBanish.clip.name);
                ghostBanish.Play();
            }
        }
        if (MaterialColor.a < 0f)
        {
            MaterialColor.a = 0f;
            StartGhostRespawnTimer();
        }
        MaterialComponent.color = MaterialColor;

        // Set the ghost into patrol mode
        bSpotted = false;
    }
    
    
    
}
