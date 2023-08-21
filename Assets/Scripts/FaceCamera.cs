using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        FaceMainCamera();
    }

    void FaceMainCamera()
    {
        if (mainCamera == null) return;

        // Calculate the direction from the text object to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Set the rotation of the text object to face the camera
        // The second argument "Vector3.up" is the upward direction, which can be adjusted as needed
        transform.rotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
    }
}