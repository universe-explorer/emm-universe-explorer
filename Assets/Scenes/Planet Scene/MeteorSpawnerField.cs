using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeteorSpawnerField : MonoBehaviour
{
    private Bounds bounds;

    public float ColliderSize { get { return _colliderOffest;  } }

    private BoxCollider _boxCollider;

    [SerializeField]
    private float _colliderOffest = 5f;

    public void SpawnMeteors(int density)
    {
        for (int i = 0; i < density; i++)
        {
            int randomMeteor = Random.Range(0, MeteorSpawnerFieldCreator.Instance.Meteors.Length);

            Instantiate(MeteorSpawnerFieldCreator.Instance.Meteors[randomMeteor], RandomPointInBounds(bounds), Quaternion.identity, gameObject.transform);
        }
    }

    private void Start()
    {
        CreateCollider();
    }

    private void CreateCollider()
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

        Bounds meteorisFiledBounds = gameObject.GetComponent<BoxCollider>().bounds;
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
            SpawnMeteors(other.GetComponent<MeteorSpawner>().Density);
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
        foreach (var item in gameObject.GetComponentsInChildren<AsteroidBehaviour>())
        {
            Destroy(item.gameObject);
        }
        
    }
}