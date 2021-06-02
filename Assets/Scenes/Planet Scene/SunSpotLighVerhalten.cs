using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpotLighVerhalten : MonoBehaviour
{
    private float _intensity = 10;
    private float _offset = 100f;

    [SerializeField]
    private Transform _target;

    private Light _light;
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.transform.LookAt(_target);
        _light.range = (Vector3.Distance(_light.transform.position, _target.transform.position) + _offset);
        _light.intensity = _light.range * (_offset / 2);
    }
}
