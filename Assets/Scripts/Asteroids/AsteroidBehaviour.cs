using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBehaviour : MonoBehaviour
{
    private float _thrust, _rotationSpeed, _orbitSpeed;
    private Vector3 _rotationVector;
    private float _mass, _drag, _angularDrag;

    private Rigidbody _asteroid;
    private bool _orbitAroundPlanet = false;
    private GameObject _parent;

    public void Setup(float thrust, float rotationSpeed, float mass, float drag, float angularDrag)
    {
        _thrust = thrust;
        _rotationSpeed = rotationSpeed;

        _rotationVector = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        _mass = mass;
        _drag = drag;
        _angularDrag = angularDrag;
    }

    public void Setup(float thrust, float rotationSpeed, float mass, float drag, float angularDrag, GameObject parent,
        float orbitSpeed)
    {
        Setup(thrust, rotationSpeed, mass, drag, angularDrag);

        _parent = parent;
        _orbitSpeed = orbitSpeed;

        _orbitAroundPlanet = true;
    }

    void Start()
    {
        _asteroid = GetComponent<Rigidbody>();

        _asteroid.mass = _mass;
        _asteroid.drag = _drag;
        _asteroid.angularDrag = _angularDrag;


        _asteroid.angularVelocity = _rotationVector *
                                    _rotationSpeed;

        _asteroid.AddForce(transform.forward * _thrust, ForceMode.Impulse);

        if (_orbitAroundPlanet)
        {
            _asteroid.isKinematic = true;
        }

        StartCoroutine(ScaleUp());
    }

    public void Remove()
    {
        StartCoroutine(ScaleDown(gameObject));
    }

    private void Update()
    {
        if (_orbitAroundPlanet)
        {
            transform.RotateAround(_parent.transform.position, _parent.transform.up, _orbitSpeed * Time.deltaTime);
            transform.Rotate(_rotationVector, _rotationSpeed * Time.deltaTime);
        }
    }

    public float maxSize = 1f;
    public float growFactor = 2f;
    public float waitTime = 100f;

    /// <summary>
    /// Scales Down a GameObject. When SCaled to zero the GO is being destroyed
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    IEnumerator ScaleDown(GameObject go)
    {
        float timer = 0;

        timer = 0;
        while (0 < transform.localScale.x)
        {
            timer += Time.deltaTime;
            transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            yield return null;
        }
        Destroy(go);
    }

    IEnumerator ScaleUp()
    {
        float timer = 0;
        transform.localScale = Vector3.zero;

        while (maxSize > transform.localScale.x)
        {
            timer += Time.deltaTime;
            transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;
            yield return null;
        }           
    }
}