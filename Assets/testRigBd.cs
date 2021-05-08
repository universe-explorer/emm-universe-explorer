using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRigBd : MonoBehaviour
{
    Rigidbody spaceShip;

    void Start()
    {
        spaceShip = GetComponent<Rigidbody>();
        spaceShip.mass = 100;
    }

    // Update is called once per frame
    void Update()
    {
        spaceShip.AddForce(transform.forward * -10);
        
    }
}