using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    private Camera cam;     // stores camera component

    // Start is called before the first frame update
    void Start()
    {
        // gets the GameObject's camera component
        cam = GetComponent<Camera>();

        // hide the mouse cursor at the centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        int size = 20;

        // centre of screen and caters for font size
        float posX = cam.pixelWidth / 2 - size / 4;
        float posY = cam.pixelHeight / 2 - size / 2;

        // displays "*" on screen
        GUI.Label(new Rect(posX, posY, size, size), "+");
    }

    // Update is called once per frame
    void Update()
    {
        // // on left mouse button click
        // if (Input.GetMouseButtonDown(0))
        // {
        //     // get point in the middle of the screen
        //     Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        //
        //     // create a ray from the point in the direction of the camera
        //     Ray ray = cam.ScreenPointToRay(point);
        //
        //     RaycastHit hit; // stores ray intersection information
        //
        //     // ray cast will obtain hit information if it intersects anything
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         // get the GameObject that was hit
        //         GameObject hitObject = hit.transform.gameObject;
        //
        //         // get that ChangeColour component
        //         ChangeColour target = hitObject.GetComponent<ChangeColour>();
        //
        //         // if the object has a ChangeColour component
        //         if (target != null)
        //         {
        //             // set the component to a random colour
        //             target.SetRandomColour();
        //         }
        //         else
        //         {
        //             // create a small sphere at the hit point
        //             StartCoroutine(SphereIndicator(hit.point));
        //         }
        //     }
        // }
    }

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        // create a small sphere at provided location
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.transform.position = pos;

        // wait for 1 second
        yield return new WaitForSeconds(1);

        // remove the sphere
        Destroy(sphere);
    }
}
