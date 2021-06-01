using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBehaviour : MonoBehaviour
{
    private float _thrust, _rotationSpeed, _orbitSpeed;
    private float _mass, _drag, _angularDrag;

    private Rigidbody _asteroid;

    public void Setup(float thrust, float rotationSpeed, float mass, float drag, float angularDrag)
    {
        _thrust = thrust;
        _rotationSpeed = rotationSpeed;

        _mass = mass;
        _drag = drag;
        _angularDrag = angularDrag;
    }

    void Start()
    {
        _asteroid = GetComponent<Rigidbody>();

        _asteroid.mass = _mass;
        _asteroid.drag = _drag;
        _asteroid.angularDrag = _angularDrag;
        
        
        _asteroid.angularVelocity = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) *
                                    _rotationSpeed;

        _asteroid.AddForce(transform.forward * _thrust, ForceMode.Impulse);
    }
}