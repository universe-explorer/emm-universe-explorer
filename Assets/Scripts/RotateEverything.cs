using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEverything : MonoBehaviour
{
    public Transform sun;
    public float _Rotation = 15f;
    public Transform ship;
    public Transform ship2;
    public float _Gravit_Rotation = 10f;
    public float _ShipSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.deltaTime * 0.4f);
        if (ship != null)
        {
            ship.transform.RotateAround(sun.position, Vector3.up, _ShipSpeed * Time.deltaTime);
        }
        if (ship2 != null)
        {
            ship2.transform.RotateAround(sun.position, Vector3.up, _Rotation * Time.deltaTime);
        }

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].transform.Rotate(new Vector3(0f, _Rotation * Time.deltaTime, 0f));
            planets[i].transform.RotateAround(sun.position, Vector3.up, _Gravit_Rotation * Time.deltaTime);
        }
    }
}
