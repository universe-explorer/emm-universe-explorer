using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeteorSpawnerField : MonoBehaviour
{
    private Vector3Int _iD;

    [Header("MeteoridField Settings")]
    private Bounds bounds;

    public float ColliderSize { get { return _colliderOffest;  } }

    private BoxCollider _boxCollider;

    [HideInInspector]
    public bool SettingsFoldout = false;

    [SerializeField]
    private float _colliderOffest = 5f;

    public AsteroidSettings AsteroidSettings;

    public void SpawnMeteors()
    {
        float asteroidScale;
        for (int i = 0; i < MeteorSpawnerFieldCreator.Instance.Meteors.Length; i++)
        {
            for (int j = 0; j < (AsteroidSettings.normalSizedAsteroids + AsteroidSettings.hugeSizedAsteroids) / MeteorSpawnerFieldCreator.Instance.Meteors.Length; j++)
            {
                asteroidScale = Random.Range(AsteroidSettings.hugeAsteroidSizeMin, AsteroidSettings.hugeAsteroidSizeMax);

                int randomMeteor = Random.Range(0, MeteorSpawnerFieldCreator.Instance.Meteors.Length);

                Transform spawnedAsteroid = Instantiate(MeteorSpawnerFieldCreator.Instance.Meteors[randomMeteor], RandomPointInBounds(bounds), Quaternion.identity, gameObject.transform);
                spawnedAsteroid.gameObject.AddComponent<AsteroidBehaviour>().Setup(Random.Range(AsteroidSettings.thrustMin, AsteroidSettings.thrustMax), Random.Range(AsteroidSettings.rotationSpeedMin, AsteroidSettings.rotationSpeedMax), AsteroidSettings._mass, AsteroidSettings._drag, AsteroidSettings._angularDrag);
                spawnedAsteroid.transform.localScale *= asteroidScale;
            }
        }
    }

    /*private void Awake()
    {
        CreateCollider();
    }*/

    public void CreateCollider()
    {
        if (gameObject.GetComponent<BoxCollider>() == null)
        {
            _boxCollider = gameObject.AddComponent<BoxCollider>();
        }
        else
        {
            _boxCollider = gameObject.GetComponent<BoxCollider>();
        }
        
        _boxCollider.size = Vector3.one *  _colliderOffest;
        _boxCollider.isTrigger = true;

        bounds = _boxCollider.bounds;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
            );
    }

    private void OnValidate()
    {
        CreateCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            Debug.Log("entered collider...");
            SpawnMeteors();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            Debug.Log("entered collider...");
            DestroyMeteors();
        }
    }

    private void DestroyMeteors()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<AsteroidBehaviour>().Remove();
        }
        
    }
}