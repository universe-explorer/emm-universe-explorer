using UnityEngine;

/**
 *  Enforce 2D Game Object to look at the main camera all the time which 
 *  makes it feel like a 3D Game Object
 */
public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Transform mainCameraTransform;

    void Start()
    {
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCameraTransform);
    }
}
