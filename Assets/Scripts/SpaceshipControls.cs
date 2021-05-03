using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceshipControls : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public float movementSpeed = 350f;

    public bool horizontalMovementInverted = false;
    public bool verticalMovementInverted = false;

    private Vector3 target_direction;
    private float angleHorizontal;
    private float angleVertical;

    private void Start()
    {
        //init spaceship position 
        //todo there is probably a better way

        transform.Rotate(5, 3.91f, 0);
        target_direction = new Vector3(0.54f, -0.094f, -0.84f);

        angleHorizontal = 0.5916f;
        angleVertical = -0.032f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        float rotationSpeedConverted = rotationSpeed * Time.deltaTime * 0.25f;
        float movementSpeedConverted = movementSpeed * Time.deltaTime * 0.25f;

        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");
        float movementInput = Input.GetAxis("Vertical");

        if (horizontalMovementInverted)
        {
            horizontalInput *= -1;
        }

        if (verticalMovementInverted)
        {
            verticalInput *= -1;
        }

        angleHorizontal += horizontalInput * rotationSpeedConverted;
        angleVertical += verticalInput * rotationSpeedConverted;

        target_direction =
            new Vector3(Mathf.Sin(angleHorizontal), Mathf.Sin(angleVertical), Mathf.Cos(angleHorizontal));
        transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.LookRotation(target_direction), 1);

        //only move forward
        if (movementInput > 0)
        {
            transform.Translate(0, 0, -(movementInput * movementSpeedConverted));
        }
    }
}