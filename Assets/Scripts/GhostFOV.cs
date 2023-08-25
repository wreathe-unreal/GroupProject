using System;
using Unity.Mathematics;
using UnityEngine;

public class GhostFOV : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;
    public LayerMask obstructionMask;

    public bool bSpotted;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        FieldOfViewCheck();
    }

    private void FieldOfViewCheck()
    {
        if (Vector3.Distance(playerRef.transform.position, transform.position) > radius)
        {
            bSpotted = false;
            return;
        }

        Vector3 directionToTarget = (playerRef.transform.position - transform.position).normalized;
        directionToTarget.y = 0;
        float dot = Vector3.Dot(transform.forward, directionToTarget) * 180f;
        
        if (dot < 180f - (angle/2f))
        {
            bSpotted = false;
            return;
        }
        
        //Debug.Log(Vector3.Dot(transform.forward, directionToTarget) * 180);

        Vector3 origin = transform.position + new Vector3(0.0f, 1.5f, 0.0f); //raise origin so we dont raycast floor
        
        float distanceToTarget = Vector3.Distance(origin, playerRef.transform.position);
        
        bSpotted = !Physics.Raycast(origin, directionToTarget, distanceToTarget, obstructionMask);
        
        Debug.DrawRay(origin, directionToTarget * distanceToTarget, Color.red, .01f);

    }
}
