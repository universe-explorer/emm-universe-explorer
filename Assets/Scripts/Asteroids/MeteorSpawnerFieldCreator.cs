using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawnerFieldCreator : MonoBehaviour
{
    [Header("FieldCreatorSettings Settings")]
    public Transform parent;

    [SerializeField]
    public Transform[] Meteors;

    private static MeteorSpawnerFieldCreator _instance;

    public Transform MeteorSpawnerField;

    [SerializeField]
    [Range(1, 20)]
    private int _range = 10;

    private List<Transform> _spawnedFields = new List<Transform>();

    /// <summary>
    /// The field in which the palyer is currently in.
    /// </summary>
    private Transform _activeField;

    private bool[,,] _FieldArray;

    private MeteorSpawnerField[,,] _FieldSciprtsArray;

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
        SpawnMeteorFields();
        _FieldArray = new bool[_range, _range, _range];
        _FieldSciprtsArray = new MeteorSpawnerField[_range, _range, _range];

        for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    _FieldArray[i, j, k] = false;
                }
            }
        }
    }

    private void SpawnMeteorFields()
    {
        ClearMeteorFields();
        int size = (int)MeteorSpawnerField.GetComponent<MeteorSpawnerField>().ColliderSize;
        bool[,] arr = new bool[_range,_range];

        /*for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    Vector3 position = new Vector3(
                        (i * ((float)size)) - (((float)size * _range) / 2) + transform.position.x,
                        (j * ((float)size)) - (((float)size * _range) / 2) + transform.position.y,
                        (k * ((float)size)) - (((float)size * _range) / 2) + transform.position.z
                        );
                    Transform created = Instantiate(MeteorSpawnerField, position, Quaternion.identity, parent);
                    created.GetComponent<MeteorSpawnerField>().CreateCollider();
                    _spawnedFields.Add(created);
                }
            }
        }*/

        for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    Vector3 position = new Vector3(
                        i * size - (_range * size / 2) + size / 2 + transform.position.x,
                        j * size - (_range * size / 2) + size / 2 + transform.position.y,
                        k * size - (_range * size / 2) + size / 2 + transform.position.z
                        );
                    Transform created = Instantiate(MeteorSpawnerField, position, Quaternion.identity, parent);
                    created.GetComponent<MeteorSpawnerField>().CreateCollider();
                    _spawnedFields.Add(created);
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

    public void NewActiveEntered(Vector3Int iD)
    {
        _FieldArray[iD.x, iD.y, iD.z] = true;
    }

    public void NewActiveLeft(Vector3Int iD)
    {
        _FieldArray[iD.x, iD.y, iD.z] = false;
    }

    private void OnDrawGizmosSelected()
    {
        int size = (int)MeteorSpawnerField.GetComponent<MeteorSpawnerField>().ColliderSize;

        for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    Vector3 position = new Vector3(
                        i * size - (_range * size / 2) + size / 2 + transform.position.x,
                        j * size - (_range * size / 2) + size / 2 + transform.position.y,
                        k * size - (_range * size / 2) + size / 2 + transform.position.z
                        );
                    Gizmos.DrawWireCube(position, size * Vector3.one);
                }
            }
        }

        /*for (int i = 0; i < _range; i++)
        {
            for (int j = 0; j < _range; j++)
            {
                for (int k = 0; k < _range; k++)
                {
                    Vector3 position = new Vector3(
                        (i * ((float)size)) - (((float)size * _range) / 2) + transform.position.x,
                        (j * ((float)size)) - (((float)size * _range) / 2) + transform.position.y,
                        (k * ((float)size)) - (((float)size * _range) / 2) + transform.position.z
                        );
                    Gizmos.DrawWireCube(position, size * Vector3.one );

                }
            }
        }*/
    }
}