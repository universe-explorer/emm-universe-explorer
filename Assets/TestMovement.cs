using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour

{
    public float MovementSpeed = .2f;

    public float RotationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.position += transform.forward * MovementSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            transform.position -= transform.forward * MovementSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation *= Quaternion.Euler(0, 0.02f, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation *= Quaternion.Euler(0, -0.02f, 0);
        }
    }
}
