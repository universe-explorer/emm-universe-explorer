using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapBehaviour : MonoBehaviour
{

    public Transform spaceShip;
    private float cameraHeight = 100;
    private float referenceHeight;

    void Start()
    {
        referenceHeight = spaceShip.position.y;
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = spaceShip.transform.position;
        newPosition.y = referenceHeight + cameraHeight;
        transform.position = newPosition;

        Quaternion newRotation = Quaternion.Euler(90, spaceShip.rotation.eulerAngles.y, spaceShip.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 0.5f);
	}

    public float getCameraHeight()
    {
        return cameraHeight;
    }


    public void setCameraHeight(float newCameraHeight)
    {
        cameraHeight = newCameraHeight;
    }

    private void CreatePlanetIcon()
    {
        // test 
    }
}
