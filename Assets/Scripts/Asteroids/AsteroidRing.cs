using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AsteroidRing : MonoBehaviour
{
    [Header("AsteroidField Settings")] public int amountOfAsteroids = 150;
    public float innerRadius;
    public float outerRadius;
    public float height;

    [Header("Asteroid Settings")] public float asteroidSizeMin = .1f;
    public float asteroidSizeMax = 2.5f;

    public float rotationSpeedMin = .25f;
    public float rotationSpeedMax = 3f;

    public float orbitSpeedMin = 0.2f;
    public float orbitSpeedMax = 2f;
    public bool userMaterialFromParent = false;


    public GameObject[] asteroidPrefabs;

    private float mass = 1, drag = 0f, angularDrag = 0f;

    private Vector3 _localPosition, _worldOffset, _worldPosition;
    private float _randomRadius, _randomRadian;
    private float _x, _y, _z;


    private void Spawn(GameObject prefab, Vector3 spawnPoint, float thrust, float rotationSpeed, float scale,
        float mass, float drag, float angularDrag, float orbitSpeed)
    {
        GameObject spawnedAsteroid = Instantiate(prefab, spawnPoint, Quaternion.identity);
        spawnedAsteroid.AddComponent<AsteroidBehaviour>().Setup(thrust, rotationSpeed, mass, drag, angularDrag,
            transform.gameObject, orbitSpeed);
        spawnedAsteroid.transform.localScale *= scale;
        spawnedAsteroid.transform.SetParent(transform);
    }

    void Start()
    {
        GameObject prefab;

        for (int j = 0; j < amountOfAsteroids; j++)
        {
            prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length - 1)];

            do //source: https://www.youtube.com/watch?v=w422-JEQ8Og
            {
                //================================================
                // Random Point on a Circle given only the Angle.
                // x = cx + r * cos(a)
                // y = cy + r* sin(a)
                //================================================

                _randomRadius = Random.Range(innerRadius, outerRadius);
                _randomRadian = Random.Range(0, (2 * Mathf.PI));

                _y = Random.Range(-(height / 2), (height / 2));
                _x = _randomRadius * Mathf.Cos(_randomRadian);
                _z = _randomRadius * Mathf.Sin(_randomRadian);
            } while (float.IsNaN(_z) && float.IsNaN(_x));

            _localPosition = new Vector3(_x, _y, _z);
            _worldOffset = transform.rotation * _localPosition;
            _worldPosition = transform.position + _worldOffset;

            Spawn(prefab, _worldPosition, 0,
                Random.Range(rotationSpeedMin, rotationSpeedMax), Random.Range(asteroidSizeMin, asteroidSizeMax), mass,
                drag, angularDrag,
                Random.Range(orbitSpeedMin, orbitSpeedMax));
        }
    }
}