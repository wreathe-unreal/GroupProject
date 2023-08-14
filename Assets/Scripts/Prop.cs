using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    private int nTimesHit = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomColour();
    }

    // Update is called once per frame
    void Update()
    {
        if (nTimesHit >= 10)
        {
            SetRandomColour();
            nTimesHit = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        nTimesHit++;
    }
    
    public void SetRandomColour()
    {
        // create a new random colour
        Color random = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

        // set material to the random colour
        GetComponent<Renderer>().material.color = random;
    }
}
