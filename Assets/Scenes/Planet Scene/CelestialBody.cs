using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{
    public enum BodyType { Planet, Moon, Sun, Asteroid }
    public BodyType Body;

    public float Radius;
    public float SurfaceGravity;
    public Vector3 InitialVelocity;
    public string BodyName = "Unnamed";
    Transform _MeshHolder;

    private Vector3 _Velocity;

    public Vector3 Velocity 
    {
        get
        {
            if (_Velocity == null)
            {
                _Velocity = InitialVelocity;
            }
            return _Velocity;
        }
        private set
        {
            _Velocity = value;
        }
    }
    public float Mass { get; private set; }
    Rigidbody _Rigidbody;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        Velocity = InitialVelocity;
        Debug.Log("new Velocity: " + Velocity);
        RecalculateMass();

    }

    /// <summary>
    /// updates the local velocity by the acceleration
    /// </summary>
    /// <param name="acceleration"></param>
    /// <param name="timeStep"></param>
    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        Velocity += acceleration * timeStep;
    }

    /// <summary>
    /// Updates the local position
    /// </summary>
    /// <param name="timeStep"></param>
    public void UpdatePosition(float timeStep)
    {
        Rigidbody.MovePosition(Rigidbody.position + Velocity * timeStep);    
    }

    private void RecalculateMass()
    {
        Mass = SurfaceGravity * Radius * Radius / Universe.GravitationalConstant;
        Rigidbody.mass = Mass;
    }

    public Rigidbody Rigidbody
    {
        get
        {
            if (!_Rigidbody)
            {
                _Rigidbody = GetComponent<Rigidbody>();
            }
            return _Rigidbody;
        }
    }

    private void OnValidate()
    {
        RecalculateMass();
        //Mass = SurfaceGravity * Radius * Radius / Universe.GravitationalConstant;
        _MeshHolder = transform.GetChild(0);
        _MeshHolder.localScale = Vector3.one * Radius;
        gameObject.name = BodyName;
    }

    /// <summary>
    /// Position of the Rigidbody
    /// </summary>
    public Vector3 Position
    {
        get
        {
            return _Rigidbody.position;
        }
    }
}
