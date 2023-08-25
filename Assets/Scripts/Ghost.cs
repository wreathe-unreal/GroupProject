using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.XR;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.UIElements;

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

    private bool inPursuit = false;
    private float pursuitTime = 0;
    [SerializeField]
    private int pursuitDur = 30;

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
    // Start is called before the first frame update
    void Start()
    {
        
        ghostMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        ghostMaterial = ghostMesh.material;
        ghostMaterialColor = ghostMaterial.color;
        ghostMaterialColor.a = .7f;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        ghostAnimate = GetComponent<Animator>();
        ghostGlow = GetComponent<Light>();
        ghostFOV = transform.GetChild(1).GetComponent<GhostFOV>();
    }

    // Update is called once per frame
    void Update()
    {
        ghostState();
        ReactToLight();

    }

    private void ghostState ()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        bSpotted = ghostFOV.bSpotted;

        //Spotted target
        if (bSpotted || dist < 1.5f)
        {
            ghostAnimate.SetTrigger("spotted");
            ghostGlow.color = Color.red;
            transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));

            agent.destination = new Vector3(player.transform.position.x, 0, player.transform.position.z);

            inPursuit = true;
            
            if (dist < 1.45f)
            {
                caught();
            }

        }
        //lost sight of target // hunting mode
        else if (bSpotted == false && inPursuit && pursuitTime < pursuitDur)
        {
            //ghostAnimate.SetTrigger("searching");
            ghostGlow.color = Color.yellow;
            transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
            agent.destination = new Vector3(player.transform.position.x, 0, player.transform.position.z);
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
                target = new Vector3(waypoints[currentPoint].position.x, 0, waypoints[currentPoint].position.z);

            }
            
            direction = target - transform.position;

            if (direction.magnitude < 1.5 && reachStart == false)
            {
                reachStart = true;
                ghostPoints();
                //currentPoint = Random.Range(0, waypoints.Length);
            }
            
            agent.destination = target;
            
            inPursuit = false;
        }
    }

    void caught()
    {
        if (!onetime && inPursuit)
        {
            //Insert Jumpscare etc

            onetime = true;
        }
    }

    //Function for random behavior when ghost is at specific waypoint
    void ghostPoints()
    {
        int rando = Random.Range(0, 3);
        //int rando = 2;
        switch (rando)
        {
            case 0:         //straight Next poimt
                Invoke("newPoint", 1.0f);
                Debug.Log("Go");
                break;
            case 1:         //idle
                //Invoke("investigate", 1.0f);
                Invoke("newPoint", 2.0f);
                Debug.Log("Go2");
                break;
            case 2:         //Looking around
                ghostAnimate.SetTrigger("looking");
                Invoke("newPoint", 4.0f);
                Debug.Log("Go3");
                break;
        }
    }

    //Set next waypoint for ghost to patrol
    void newPoint()
    {
        currentPoint = Random.Range(0, waypoints.Length);
        while (currentPoint == prevPoint)
        {
            currentPoint = Random.Range(0, waypoints.Length);
        }
        prevPoint = currentPoint; 
        
        reachStart = false;
    }

    void ReactToLight()
    {
        MeshRenderer ghostMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        Material ghostMaterial = ghostMesh.material;

        // Make sure the shader used by the material supports transparency
        
        ghostMaterialColor = ghostMaterial.color;
        ghostMaterial.SetFloat("_Mode", 3); // Set to Transparent mode
        ghostMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        ghostMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        ghostMaterial.SetInt("_ZWrite", 0);
        ghostMaterial.DisableKeyword("_ALPHATEST_ON");
        ghostMaterial.EnableKeyword("_ALPHABLEND_ON");
        ghostMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        ghostMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        // Sphere projection towards the player's position
        Vector3 directionToGhost = (transform.position - player.transform.position).normalized;

        // Dot product to check if the player is facing the ghost
        float dot = Vector3.Dot(player.transform.forward, directionToGhost);

        // If dot product is positive, player's forward is in the same general direction as directionToGhost
        if (dot > 0)
        {
            // Raycast to check for obstruction
            float distanceToGhost = Vector3.Distance(transform.position, player.transform.position);
            
            if (!Physics.Raycast(player.transform.position, directionToGhost, distanceToGhost, obstructionMask))
            {
                if (player.GetComponent<PlayerCharacter>().bFlashlightActive)
                {
                    Debug.Log("Player is facing the ghost and not obstructed.");

                    // Adjust the alpha value
                    ghostMaterialColor.a = Mathf.Lerp(ghostMaterialColor.a, 0, Time.deltaTime * 1.0f);
                    ghostMaterial.color = ghostMaterialColor;

                    // Set the ghost into patrol mode
                    bSpotted = false;
                } 
            }
            else
            {
                // Obstruction detected
                Debug.Log("Player is facing the ghost, but there is an obstruction.");
                ghostMaterialColor.a = Mathf.Lerp(ghostMaterialColor.a, 1, Time.deltaTime * 1.0f);
                ghostMaterial.color = ghostMaterialColor;
            }
        }
        else
        {
            // Player is not facing the ghost
            Debug.Log("Player is not facing the ghost.");
            ghostMaterialColor.a = Mathf.Lerp(ghostMaterialColor.a, 1, Time.deltaTime * 1.0f);
            ghostMaterial.color = ghostMaterialColor;
        }
       
    }

}
