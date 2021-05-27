using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidField : MonoBehaviour
{
    public int hugeSizedAsteroids = 15;
    public int normalSizedAsteroids = 150;
    
    public float normalAsteroidSizeMin = .5f;
    public float normalAsteroidSizeMax = 2.5f;
    
    public float hugeAsteroidSizeMin = 3f;
    public float hugeAsteroidSizeMax = 9f;

    public GameObject[] asteroidPrefabs;

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


    void Start()
    {
        Bounds asteroidFieldBounds = gameObject.GetComponent<BoxCollider>().bounds;

        int randomPrefab;
        var emptyObject = new GameObject();

        float randomAsteroidScale;
        for (int i = 0; i < normalSizedAsteroids + hugeSizedAsteroids; i++)
        {

            if (i < normalSizedAsteroids)
            {
                randomAsteroidScale = Random.Range(normalAsteroidSizeMin, normalAsteroidSizeMax);
            }
            else
            {
                randomAsteroidScale = Random.Range(hugeAsteroidSizeMin, hugeAsteroidSizeMax);
            }
            
            randomPrefab = Random.Range(0, asteroidPrefabs.Length);
            Vector3 spawnPoint = RandomPointInBounds(asteroidFieldBounds);

            GameObject spawnedAsteroid =
                Instantiate(asteroidPrefabs[randomPrefab], spawnPoint, Quaternion.identity);

            //set random scale
            spawnedAsteroid.transform.localScale *= randomAsteroidScale;

            //prevent scaling asteroids to AsteroidField - scale
            emptyObject.transform.parent = gameObject.transform;
            spawnedAsteroid.transform.parent = emptyObject.transform;
        }
    }
}