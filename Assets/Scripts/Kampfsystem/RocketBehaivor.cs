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

    [SerializeField]
    [Tooltip("Time to live in seconds")]
    private float _timeToLive = 10f;

    private Transform _target;

    [SerializeField]
    private float _DetectionRadius = 10f;

    private float _speed;

    /// <summary>
    /// Sets the Initial Direction of the Missle
    /// </summary>
    /// <param name="direction"></param>
    public void SetUp(Vector3 direction)
    {
        _launchDirection = direction;
    }

    private void OnValidate()
    {
        GetComponent<SphereCollider>().radius = _DetectionRadius;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = _DetectionRadius;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddExplosionForce(2f, transform.position, 2f);


        StartCoroutine(RocketDestroyer());
    }

    IEnumerator RocketDestroyer()
    {
        yield return new WaitForSeconds(_timeToLive);
        RocketDestroyAnimator();
    }

    private void RocketDestroyAnimator()
    {

    }

    internal void Engage(float speed)
    {

        _engaged = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force;
        if (_engaged)
        {
            if (_target != null)
            {
                force = (_target.position - transform.position).normalized;
            }
            else
            {
                force = transform.forward;
            }
            rb.AddForce(force * _speed, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            _target = other.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _DetectionRadius);
    }
}
