using System;
using System.Collections;
using System.Collections.Generic;
using SerialCommunication;
using UnityEngine;

[ExecuteInEditMode]
public class AtmosphereController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    // Start is called before the first frame update
    
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(_target);

    }
}