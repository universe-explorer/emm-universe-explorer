using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Rotates transform
 */
public class RotateScript : MonoBehaviour
{

    private Transform t;
    [SerializeField] private Vector3 rotationVector;
    
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        t.Rotate(rotationVector * Time.deltaTime, Space.Self);
    }
}
