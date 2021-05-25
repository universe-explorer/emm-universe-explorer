using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] _Bodies;
    static NBodySimulation _Instance;

    private void Awake()
    {
        _Bodies = GameObject.FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.PhysicsTimeStep;
        Debug.Log("Setting fixed delta time to: " + Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < _Bodies.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(_Bodies[i].Position, _Bodies[i]);
            _Bodies[i].UpdateVelocity(acceleration, Universe.PhysicsTimeStep);
        }
        for (int i = 0; i < _Bodies.Length; i++)
        {
            _Bodies[i].UpdatePosition(Universe.PhysicsTimeStep);
        }
    }

    private Vector3 CalculateAcceleration(Vector3 point, CelestialBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in _Bodies)
        {
            if (body != ignoreBody)
            {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * Universe.GravitationalConstant * body.Mass / sqrDst;
            }
        }
        return acceleration;
    }

    public static CelestialBody[] Bodies
    {
        get 
            {
            return Instance._Bodies;
            } 
    }

    static NBodySimulation Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<NBodySimulation>();

            }
            return _Instance;
        }
    }
}
