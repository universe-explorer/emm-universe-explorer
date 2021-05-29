using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    private const float maxVelocity = 50;
    private const float defaultForce = 1;
    private const float rollMultiplier = 50;
    private const float fullRoll = 360;
    private const float rollPerFrame = 8;

    private Rigidbody _ship;
    private Vector3 _movementDirection;
    private int _currentBoostTime;
    private float _currentRoll;
    private bool _isRolling;
    private float _rollingDirection;

    private float _verticalInput;
    private Vector2 _mouseInput;

    public float _boostMultiplier;
    public int _maxBoostDuration;

    void Start()
    {
        _ship = gameObject.GetComponent<Rigidbody>();
        _boostMultiplier = 1.5f;
        _maxBoostDuration = 120;
    }

    void FixedUpdate()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _mouseInput.x += Input.GetAxis("Mouse X");
        _mouseInput.y += Input.GetAxis("Mouse Y");


        if (Input.GetKeyDown(KeyCode.A))
        {
            Roll(defaultForce * -rollMultiplier);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Roll(defaultForce * rollMultiplier);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Boost();
        }
        else if (_currentBoostTime > 0)
        {
            _currentBoostTime--;
        }

        if (_isRolling)
        {
            _currentRoll += rollPerFrame * _rollingDirection;
            Quaternion _shipRotation = transform.rotation;

            Vector3 rollDirection = new Vector3(0, 1, 0);

            transform.Rotate(new Vector3(0, 0, rollPerFrame * -_rollingDirection), Space.Self);

            if (_currentRoll == fullRoll || _currentRoll == -fullRoll)
            {
                _isRolling = false;
                _currentRoll = 0;
            }
        }

        Move(_verticalInput);
        Rotate(_mouseInput);
    }


    /// <summary> 
    ///   <param name="direction"> Movement direction</param>
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
    public void Move(Vector3 direction, float force)
    {
        float _forwardMovement = Vector3.Dot(transform.forward, _movementDirection);
        Vector3 movementDirectionTemp = transform.forward * _forwardMovement;
        movementDirectionTemp += transform.forward * force;

        if (movementDirectionTemp.magnitude <= maxVelocity)
        {
            _movementDirection = movementDirectionTemp;
        }

        _ship.velocity = -movementDirectionTemp;
    }

    /// <summary> 
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
    public void Move(float force)
    {
        Move(transform.forward, force);
    }

    /// <summary> 
    ///   <param name="direction"> Direction for the spaceship to rotate</param>
    ///   <param name="angle"> Rotation amount</param>
    /// </summary>
    public void Rotate(Vector2 mouseInput)
    {
        _ship.rotation = Quaternion.Euler(mouseInput.y, mouseInput.x, 0f);
    }

    /// <summary> 
    ///   <param name="force"> Negative amount for left roll, positive amount for right roll</param>
    /// </summary>
    public void Roll(float force)
    {
        if (!_isRolling && force != 0)
        {
            if (force > 0)
            {
                _rollingDirection = 1;
            }
            else
            {
                _rollingDirection = -1;
            }

            float _forwardMovement = Vector3.Dot(transform.forward, _movementDirection);
            _movementDirection = transform.forward * _forwardMovement;

            _movementDirection += transform.right * force;

            _isRolling = true;
            _currentRoll = 0;
        }
    }

    public void Boost()
    {
        if (_currentBoostTime < _maxBoostDuration)
        {
            Move(defaultForce * _boostMultiplier);
            _currentBoostTime++;
        }
    }
}