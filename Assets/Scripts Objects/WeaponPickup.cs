using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.localPosition, Vector3.up, 70.0f*Time.deltaTime);
    }
    
    //coroutine
    private IEnumerator RespawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            gameObject.SetActive(true);
            break;
        }
        
    }

    void OnTriggerEnter(Collider collider)
    {
        
        Transform socket = collider.transform.GetChild(0).GetChild(0);

        if (socket.childCount > 0) //if we already  have a weapon
        {
            Destroy(socket.GetChild(0).gameObject);
        }
        Instantiate(weapon, socket);
        
        
        StartCoroutine(RespawnTimer());
        gameObject.SetActive(false);
    }
    
}
