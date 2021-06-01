using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform spaceShip;
    public float cameraOffset = 10.0f;
    public float cameraOffsetHeight = 2.5f;

    void Update()
    {
        var cam = transform;

        cam.position = spaceShip.transform.position - spaceShip.transform.forward * cameraOffset;
        cam.position += new Vector3(0, cameraOffsetHeight, 0);

        cam.rotation = Quaternion.LookRotation(spaceShip.transform.position - cam.position, Vector3.up);
    }
}