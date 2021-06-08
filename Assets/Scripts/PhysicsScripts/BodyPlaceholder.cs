using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPlaceholder : MonoBehaviour
{
    public int TerrainResolution = 50;
    public Material Material;
    public bool UseBodySettings;
    //public CelestialBody
    Mesh _Mesh;

    bool _SettingsChanged;

    private void Update()
    {
        if (_SettingsChanged)
        {
            _SettingsChanged = false;
            if (_Mesh == null)
            {
                _Mesh = new Mesh();
            }
            else
            {
                _Mesh.Clear();
            }

            //SphereMesh sphere 
        }
    }
}
