using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AsteroidSettings : ScriptableObject
{
    [Header("Asteroid Settings")]

    public int hugeSizedAsteroids = 15;
    public int normalSizedAsteroids = 150;

    public float normalAsteroidSizeMin = .5f;
    public float normalAsteroidSizeMax = 2.5f;

    public float hugeAsteroidSizeMin = 3f;
    public float hugeAsteroidSizeMax = 9f;


    [Header("Asteroidbehaviour Settings")]

    public float rotationSpeedMin = .25f;
    public float rotationSpeedMax = 3f;

    public float thrustMin = .1f;
    public float thrustMax = .5f;


    public float _mass = 1, _drag = 0.001f, _angularDrag = 0.0001f;
}
