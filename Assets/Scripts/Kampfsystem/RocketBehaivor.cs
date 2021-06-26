using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketBehaivor : MonoBehaviour
{
    [SerializeField]
    private Vector3 _launchDirection;
   
    private Rigidbody rb;

    [SerializeField]
    private bool _engaged = false;

    /// <summary>
    /// Sets the Initial Direction of the Missle
    /// </summary>
    /// <param name="direction"></param>
    public void SetUp(Vector3 direction)
    {
        _launchDirection = direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddExplosionForce(2f, transform.position, 2f);
    }

    internal void Engage()
    {
        _engaged = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_engaged)
        {
            rb.AddForce(transform.forward, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
