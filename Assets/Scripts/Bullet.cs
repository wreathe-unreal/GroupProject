using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool bExplodes;
    public Light Explosion;
    public float ExplosionForce = 100f;
    public float ExplosionRadius = 5.0f;
    public float ExplosionDelay = 3.0f;

    private bool bHasExploded = false;
    private float ExplosionTimer = 0f;
    private Light ActiveExplosion;

    // Start is called before the first frame update
    void Start()
    {
        ExplosionTimer = ExplosionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(bHasExploded)
        {
            ActiveExplosion.intensity -= Time.deltaTime * 20;
        }
        
        
        ExplosionTimer -= Time.deltaTime;
        
        if (ExplosionTimer <= 0f && !bHasExploded && bExplodes)
        {
            Explode();
            bHasExploded = true;
        }
    }
    
    void Explode()
    {
        ActiveExplosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        // Get nearby objects
        Collider[] Collisions = Physics.OverlapSphere(transform.position, ExplosionRadius);

        for(int i = 0; i < Collisions.Length; i++)
        {
            // Try to get a Rigidbody component
            Rigidbody RigidBodyComp = Collisions[i].GetComponent<Rigidbody>();
            if (RigidBodyComp != null)
            {
                // Add force
                RigidBodyComp.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                
                Color random = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                // set material to the random colour
                Collisions[i].GetComponent<Renderer>().material.color = random;
            }
        }

        // Destroy grenade
        Destroy(gameObject, 2f);
    }

        
}
