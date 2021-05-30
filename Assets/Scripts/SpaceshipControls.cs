using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    private const float maxVelocity = 50;

    private const float defaultMovementForce = 1;

    private const float defaultRollingForce = 30;
    private const float fullRoll = 360;
    private const float rollPerFrame = 8;

    private const float maxZRotation = 35;
    private const float zRotationSpeed = 3.5f;

    private Rigidbody _ship;
    private int _currentBoostTime;
    private float _currentRoll;
    private bool _isRolling;
    private float _rollingDirection;

    private float _verticalInput;
    private Vector2 _mouseInput;
    private Vector2 _mouseInputAngles;
    private Vector2 _mouseInputAnglesClamped;

    public float _boostMultiplier = 1.5f;
    public int _maxBoostDuration = 120;

    void Start()
    {
        _ship = gameObject.GetComponent<Rigidbody>();

        transform.rotation = Quaternion.identity;
        _ship.velocity = transform.forward;

        Screen.lockCursor = true;
    }

    void FixedUpdate()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _mouseInput.x += Input.GetAxis("Mouse X");
        _mouseInput.y += Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.A))
        {
            Roll(defaultMovementForce * -defaultRollingForce);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Roll(defaultMovementForce * defaultRollingForce);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Boost();
        }
        else if (_currentBoostTime > 0)
        {
            _currentBoostTime--;
        }


        //perform rolling rotation
        if (_isRolling)
        {
            _currentRoll += rollPerFrame * _rollingDirection;

            transform.Rotate(new Vector3(0, 0, rollPerFrame * _rollingDirection), Space.Self);

            if (_currentRoll == fullRoll || _currentRoll == -fullRoll)
            {
                _isRolling = false;
                _currentRoll = 0;
            }
        }


        if (!_isRolling)
        {
            Move(_verticalInput);
        }
    }


    /// <summary> 
    ///   <param name="direction"> Movement direction</param>
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
    public void Move(Vector3 direction, float force)
    {
        float _forwardMovement = Vector3.Dot(transform.forward, _movementDirection);
        Vector3 movementDirectionTemp = direction * _forwardMovement;
        movementDirectionTemp += direction * force;

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
        _mouseInputAngles.x += mouseInput.x;
        _mouseInputAngles.y += mouseInput.y;

        //limit rotation to certain degree
        _mouseInputAnglesClamped.x += mouseInput.x;
        _mouseInputAnglesClamped.x = Mathf.Clamp(_mouseInputAnglesClamped.x, -maxZRotation, maxZRotation);

        //loop back to zero, used for spaceShip-wing rotation
        _mouseInputAnglesClamped.x = Mathf.Lerp(_mouseInputAnglesClamped.x, 0, Time.deltaTime * zRotationSpeed);


        _ship.rotation = Quaternion.Euler(_mouseInputAngles.y, _mouseInputAngles.x, _mouseInputAnglesClamped.x);
        // transform.rotation = Quaternion.Euler(_mouseInput.y, _mouseInput.x, _currentRoll); //todo rotating isn't possible while rolling: possible solution
    }

    /// <summary> 
    ///   <param name="force"> Negative amount for left roll, positive amount for right roll</param>
    /// </summary>
    public void Roll(float force)
    {
        if (!_isRolling && force != 0)
        {
            _isRolling = true;
            _currentRoll = 0;

            if (force > 0)
            {
                _rollingDirection = 1;
            }
            else
            {
                _rollingDirection = -1;
            }

            Move(transform.forward + (transform.right * _rollingDirection), Math.Abs(force));
        }
    }

    public void Boost()
    {
        if (_currentBoostTime < _maxBoostDuration)
        {
            _actualMaxVelocity =
                maxVelocity * _boostMultiplier; //set maximal velocity to default times boost multiplier
            Move(defaultMovementForce * _boostMultiplier);
            _currentBoostTime++;
        }
    }
}