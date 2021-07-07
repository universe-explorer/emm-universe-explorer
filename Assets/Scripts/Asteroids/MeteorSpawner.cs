using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to a camera
/// </summary>
[ExecuteInEditMode]
public class MeteorSpawner : MonoBehaviour
{
    private SphereCollider _sphereCollider;

    [SerializeField]
    [Tooltip("The Density of Meteors around the Camera")]
    private int _density;

    public int Density { get { return _density;  } }

    [SerializeField]
    [Tooltip("The Radius in wich meteors are being spawned")]
    private float _radius = 10f;


    // Start is called before the first frame update
    void Start()
    {
        CreateCollider();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void CreateCollider()
    {
        if (gameObject.GetComponent<SphereCollider>() == null)
        {
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        else
        {
            _sphereCollider = gameObject.GetComponent<SphereCollider>();
        }

        _sphereCollider.radius = _radius;
    }
}
