using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inspiriert von: https://www.theappguruz.com/blog/create-homing-missiles-in-game-unity-tutorial
[RequireComponent(typeof(Rigidbody))]
public class RocketBehaivor : MonoBehaviour
{
    [SerializeField]
    private Transform _expolsionVFX;

    [SerializeField]
    private Vector3 _launchDirection;
   
    private Rigidbody rb;

    private bool _engaged = false;

    [SerializeField]
    [Tooltip("Time to live in seconds")]
    private float _timeToLive = 10f;

    private Transform _target;

    [SerializeField]
    private float _DetectionRadius = 10f;

    private float _speed;

    private float _damage;

    private Target _projectileTarget;

    /// <summary>
    /// Sets the Initial Direction of the Missle
    /// </summary>
    /// <param name="direction"></param>
    public void SetUp(float damage, Target art)
    {
        _damage = damage;
        _projectileTarget = art;
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

        rb.velocity += transform.forward;
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
                rb.velocity += transform.forward * _speed;
                return;
            }
            else
            {
                Vector3 direction = _target.position - rb.position;
                direction.Normalize();
                Vector3 rateAmount = Vector3.Cross(transform.forward, direction);

                rb.angularVelocity = _angleChangingSpeed * rateAmount;
                rb.velocity += transform.forward * _speed;
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
        Destroy(gameObject);
        Destroy(Instantiate(_expolsionVFX, rb.position, Quaternion.identity).gameObject, 4f);
    }

    internal void Engage(float speed)
    {
        _speed = speed;
        _engaged = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_projectileTarget == Target.Enemy)
        {
            if (other.tag == "Enemy")
            {
                _target = other.transform;
            }

        }
        else
        {
            if (other.tag == "Player")
            {
                _target = other.transform;
            }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_projectileTarget == Target.Enemy)
        {
            if (collision.transform.tag == "EnemyHealthCollider")
            {
                collision.transform.GetComponentInParent<CombatControllerEnemy>().TakeDamage(_damage);
                //other.gameObject.gameObject.GetComponent<CombatControllerEnemy>().TakeDamage(_damage);
                Destroy(gameObject);
                Destroy(Instantiate(_expolsionVFX, collision.GetContact(0).point, Quaternion.identity).gameObject, 4f);
                return;
            }
        }
        else if (_projectileTarget == Target.Allied)
        {
            if (collision.transform.tag == "Player")
            {
                Debug.Log("Projectile: Touched Player!!!!");
                if (collision.transform.GetComponentInParent<CombatControllerPlayer>() != null)
                {
                    collision.transform.GetComponentInParent<CombatControllerPlayer>().TakeDamage(_damage);
                }
                //other.gameObject.gameObject.GetComponent<CombatControllerEnemy>().TakeDamage(_damage);
                Destroy(gameObject);
                Destroy(Instantiate(_expolsionVFX, collision.GetContact(0).point, Quaternion.identity).gameObject, 4f);
                return;
            }
        }
        else
        {
            Debug.Log("WRONG STATE:::");
        }

        if (collision.transform.tag != "EnemyHealthCollider" && collision.transform.tag != "Player" && collision.transform.tag != "Projectile" && collision.transform.tag != "PlayerHealthCollider" && collision.transform.tag != "Enemy")
        {
            //make shure, rockets explode even when touching another collider
            Destroy(gameObject);
            Destroy(Instantiate(_expolsionVFX, collision.GetContact(0).point, Quaternion.identity).gameObject, 4f);
        }
    }
}
