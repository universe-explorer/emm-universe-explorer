using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidField : MonoBehaviour
{
    [Header("AsteroidField Settings")] 
    
    public int hugeSizedAsteroids = 15;
    public int normalSizedAsteroids = 150;

    public float normalAsteroidSizeMin = .5f;
    public float normalAsteroidSizeMax = 2.5f;

    public float hugeAsteroidSizeMin = 3f;
    public float hugeAsteroidSizeMax = 9f;


    [Header("Asteroid Settings")] 
    
    public float rotationSpeedMin = .25f;
    public float rotationSpeedMax = 3f;

    public float thrustMin = .1f;
    public float thrustMax = .5f;


    public float _mass = 1, _drag = 0.001f, _angularDrag = 0.0001f;

    public GameObject[] asteroidPrefabs;


    private GameObject emptyObject;

    /**
     * Get random Point inside of bounds
     */
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }


    private void Spawn(GameObject prefab, Vector3 spawnPoint, float thrust, float rotationSpeed, float scale)
    {
        GameObject spawnedAsteroid = Instantiate(prefab, spawnPoint, Quaternion.identity);
        spawnedAsteroid.AddComponent<AsteroidBehaviour>().Setup(thrust, rotationSpeed, _mass, _drag, _angularDrag);

        spawnedAsteroid.transform.localScale *= scale;

        //prevent scaling asteroids to AsteroidField - scale
        emptyObject.transform.parent = gameObject.transform;
        spawnedAsteroid.transform.parent = emptyObject.transform;
    }

    void Start()
    {
        emptyObject = new GameObject();
        Bounds asteroidFieldBounds = gameObject.GetComponent<BoxCollider>().bounds;

        float asteroidScale;

        for (int i = 0; i < asteroidPrefabs.Length; i++)
        {
            for (int j = 0; j < (normalSizedAsteroids + hugeSizedAsteroids) / asteroidPrefabs.Length; j++)
            {
                if (j < normalSizedAsteroids / asteroidPrefabs.Length)
                {
                    asteroidScale = Random.Range(normalAsteroidSizeMin, normalAsteroidSizeMax);
                }
                else
                {
                    asteroidScale = Random.Range(hugeAsteroidSizeMin, hugeAsteroidSizeMax);
                }

                Vector3 spawnPoint = RandomPointInBounds(asteroidFieldBounds);

                Spawn(asteroidPrefabs[i], spawnPoint, Random.Range(thrustMin, thrustMax),
                    Random.Range(rotationSpeedMin, rotationSpeedMax), asteroidScale);
            }
        }
    }
}