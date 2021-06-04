using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Transform mainCameraTransform;

    // Start is called before the first frame update
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
