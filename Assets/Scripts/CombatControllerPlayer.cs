using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerPlayer : AbstractCombatController
{
    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void HealthChanged()
    {
        throw new System.NotImplementedException();
    }
}
    /*public LineRenderer _LineRenderer;

    public float _LaserWidth = 0.1f;
    public float _LaserMaxLength;
    // Start is called before the first frame update
    void Start()
    {
        _LaserMaxLength = _MaxShootDistance;
        _LineRenderer = GetComponent<LineRenderer>();
        _MaxHealth = _Health;

        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        _LineRenderer.SetPositions(initLaserPositions);
        _LineRenderer.SetWidth(_LaserWidth, _LaserWidth);
    }

    // Update is called once per frame
    void Update()
    {
        if (_CannShoot == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _Ammo--;
                RaycastHit hit;
                _LineRenderer.enabled = true;
                if (Physics.Raycast(transform.position, -transform.forward, out hit, _MaxShootDistance))
                {
                    if (hit.collider.tag == "Schiff")
                    {
                        hit.collider.gameObject.GetComponent<AbstractCombatController>().TakeDamage(_Damage);
                    }

                    if (hit.collider.GetComponent<Rigidbody>() != null)
                    {
                        hit.collider.GetComponent<Rigidbody>().AddForce((hit.collider.transform.position - transform.position) * 10f);
                    }

                    _LineRenderer.SetPosition(0, transform.position);
                    _LineRenderer.SetPosition(1, hit.collider.transform.position);
                    

                    Debug.Log("Hit: " + hit.collider.name);
                }
            }
            else
            {
                _LineRenderer.enabled = false;
            }
        }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void HealthChanged()
    {
        //Todo: change Slider in UI...
    }
}*/
