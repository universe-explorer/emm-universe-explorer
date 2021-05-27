using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    public float tumbleMin = .25f;
    public float tumbleMax = 3f;

    public float thrustMin = .1f;
    public float thrustMax = .5f;

    private Rigidbody asteroid;
    private float tumble, thrust;

    void Start()
    {
        asteroid = GetComponent<Rigidbody>();
        
        tumble = Random.Range(tumbleMin, tumbleMax);
        thrust = Random.Range(thrustMin, thrustMax);
        
        //rotate asteroid
        asteroid.angularVelocity = Vector3.one * tumble;

        //move asteroid
        asteroid.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }
}