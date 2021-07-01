using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspiriert von: https://www.theappguruz.com/blog/create-homing-missiles-in-game-unity-tutorial
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

    [SerializeField]
    private float _angleChangingSpeed = 5f;

    private void FixedUpdate()
    {
        if (_engaged)
        {
            if (_target == null)
            {
                //rb.AddForce(transform.forward * _speed, ForceMode.Acceleration);
                rb.velocity = transform.forward * _speed;
                return;
            }
            else
            {
                Vector3 direction = _target.position - rb.position;
                direction.Normalize();
                Vector3 rateAmount = Vector3.Cross(transform.forward, direction);

                rb.angularVelocity = _angleChangingSpeed * rateAmount;
                rb.velocity = transform.forward * _speed;
            }
        }
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
        _speed = speed;
        _engaged = true;
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
        if (_target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_target.position, 3f);
        }
    }
}
