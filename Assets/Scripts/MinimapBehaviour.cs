using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapBehaviour : MonoBehaviour
{

    public Transform spaceShip;
    private float cameraHeight;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    private void UpdatePosition()
    {
		
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
