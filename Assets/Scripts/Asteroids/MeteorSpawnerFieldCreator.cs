using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawnerFieldCreator : MonoBehaviour
{
    public Transform parent;

    [SerializeField]
    public Transform[] Meteors;

    private static MeteorSpawnerFieldCreator _instance;

    public Transform MeteorSpawnerField;

    [SerializeField]
    [Range(1, 20)]
    private int _range = 10;

    private List<Transform> _spawnedMeteors = new List<Transform>();

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Start()
    {
        SpawnMeteorFields();
    }

    private void SpawnMeteorFields()
    {
        ClearMeteorFields();
        int size = (int)MeteorSpawnerField.GetComponent<MeteorSpawnerField>().ColliderSize;
        int[] arr = new int[_range];


        for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    Vector3 position = new Vector3(
                        (i * ((float)size)) - (((float)size * _range) / 2),
                        (j * ((float)size)) - (((float)size * _range) / 2),
                        (k * ((float)size)) - (((float)size * _range) / 2)
                        );
                    _spawnedMeteors.Add(Instantiate(MeteorSpawnerField, position, Quaternion.identity, parent));

                }
            }
        }
    }

    private void ClearMeteorFields()
    {
        foreach (var field in GameObject.FindGameObjectsWithTag("MeteorSpawnerField"))
        {
            //Destroy(field);
            Destroy(field.gameObject);
        }
    }

    public static MeteorSpawnerFieldCreator Instance
    {
        get
        {
            return _instance;
        }
    }

    private void OnValidate()
    {

    }

    private void OnDrawGizmosSelected()
    {
        int size = (int)MeteorSpawnerField.GetComponent<MeteorSpawnerField>().ColliderSize;
        int[] arr = new int[_range];
       
        for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    Vector3 position = new Vector3(
                        (i * ((float)size)) - (((float)size * _range) / 2),
                        (j * ((float)size)) - (((float)size * _range) / 2),
                        (k * ((float)size)) - (((float)size * _range) / 2)
                        );
                    Gizmos.DrawWireCube(position, size * Vector3.one );

                }
            }
        }
    }
}