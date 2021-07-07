using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is based on a Script by Sebastian Lague
/// His GitHub Page: https://github.com/SebLague/Solar-System/tree/Episode-03/Assets/Scripts/Celestial
/// </summary>
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

    /// <summary>
    /// The Velocity of the rb
    /// </summary>
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
        RecalculateMass();

    }

    /// <summary>
    /// updates the local velocity by the acceleration
    /// </summary>
    /// <param name="acceleration"></param>
    public void UpdateVelocity(Vector3 acceleration)
    {
        Velocity += acceleration;
    }

    /// <summary>
    /// Updates the local position
    /// </summary>
    /// <param name="scalar"></param>
    public void UpdatePosition(float scalar)
    {
        Rigidbody.MovePosition(Rigidbody.position + Velocity * scalar);
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
