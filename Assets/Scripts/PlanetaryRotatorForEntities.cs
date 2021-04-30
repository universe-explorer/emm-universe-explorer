using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetaryRotatorForEntities : MonoBehaviour
{
    public Transform ship;
    public float _ShipSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ship.transform.RotateAround(transform.position, Vector3.up, _ShipSpeed * Time.deltaTime * -1);
    }
}
