using System;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform spaceShip;
    public float offsetX = 10.0f;
    public float offsetZ = 2.5f;
    public float rotationOffsetX = -8f;

    private GameObject _spaceShipGameObject;

    private SpaceshipControls _spaceshipControlls;
    private bool _isRolling = false;
    private Vector3 _spaceShipUpVector;

    private void Start()
    {
        _spaceShipGameObject = spaceShip.gameObject;
        _spaceshipControlls = _spaceShipGameObject.GetComponent<SpaceshipControls>();
        _spaceShipUpVector = spaceShip.up;
    }

    void Update()
    {
        _isRolling = _spaceshipControlls.getIsRolling();

        if (!_isRolling)
        {
            _spaceShipUpVector = spaceShip.up;
        }

        var cam = transform;

        cam.position = spaceShip.transform.position - spaceShip.transform.forward * offsetX;
        cam.position += spaceShip.transform.up * offsetZ;

        cam.rotation = Quaternion.LookRotation(spaceShip.transform.position - cam.position, _spaceShipUpVector);
        cam.Rotate(new Vector3(rotationOffsetX, 0f, 0f), Space.Self);
    }
}