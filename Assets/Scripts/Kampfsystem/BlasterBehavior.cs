using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Laser Blaster Behavior is attached to the Laser Blaster Projectile
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BlasterBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform _expolsionVFX;

    [SerializeField]
    private Vector3 _launchDirection;

    private Rigidbody rb;

    [SerializeField]
    [Tooltip("Time to live in seconds")]
    private float _timeToLive = 10f;

    private float _speed;

    private float _damage;

    private Target _projectileTarget;

    /// <summary>
    /// Sets the Initial Direction of the Missle
    /// </summary>
    /// <param name="direction"></param>
    public void SetUp(float damage, Target art, float speed)
    {
        _damage = damage;
        _projectileTarget = art;
        _speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.AddExplosionForce(2f, transform.position, 2f);

        StartCoroutine(RocketDestroyer());

        if (_projectileTarget == Target.Allied)
        {
            //GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
            GetComponentInChildren<MeshRenderer>().material.SetVector("_EmissionColor", (Vector4)new Color(Color.red.r * 200, Color.red.g * 200, Color.red.b * 200));
        }
        else if (_projectileTarget == Target.Enemy)
        {
            GetComponentInChildren<MeshRenderer>().material.SetVector("_EmissionColor", (Vector4)new Color(Color.blue.r * 200, Color.blue.g * 200, Color.blue.b * 200));
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * _speed;
    }

    IEnumerator RocketDestroyer()
    {
        yield return new WaitForSeconds(_timeToLive);
        RocketDestroyAnimator();
    }

    private void RocketDestroyAnimator()
    {
        Destroy(gameObject);
        //Destroy(Instantiate(_expolsionVFX, rb.position, Quaternion.identity).gameObject, 4f);
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

        if (collision.transform.tag != "EnemyHealthCollider" && collision.transform.tag != "Player" && collision.transform.tag != "Projectile")
        {
            //make shure, rockets explode even when touching another collider
            Destroy(gameObject);
            Destroy(Instantiate(_expolsionVFX, collision.GetContact(0).point, Quaternion.identity).gameObject, 4f);
        }
    }
}
