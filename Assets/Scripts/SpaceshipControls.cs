using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    /* move */
    private const float maxVelocity = 50;
    private const float defaultVelocity = 5;
    private float _actualMaxVelocity = maxVelocity; //modified by boost
    
    /* rotate */
    private const float maxZRotation = 35;
    private const float zRotationSpeed = 3.5f;

    
    /* boost */
    private int _currentBoostTime;
    
    
    /* rolling */
    private const float defaultRollingForce = 2.5f;
    private const float fullRoll = 360;
    private const float rollPerFrame = 8;
    
    private float _currentRoll;
    private bool _isRolling;
    private float _rollingDirection;

    
    /* input */
    public bool useAlternativeMouseInput = false;
    public float _boostMultiplier = 1.5f;
    public int _maxBoostDuration = 120;
    
    private float _verticalInput;
    private Vector2 _mouseInput;
    private Vector2 _mouseInputAngles;
    private Vector2 _mouseInputAnglesClamped;
    
    
    /* other */
    private Rigidbody _ship;

    /// <summary>
    ///   <para>Maps value from original range to new range</para>
    ///   <param name="value"> Original value</param>
    ///   <param name="fromMin"></param>
    ///   <param name="fromMax"></param>
    ///   <param name="toMin"></param>
    ///   <param name="toMax"></param>
    /// </summary>
    float MapFloat(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (toMin - fromMin) * (toMax - fromMax) + fromMax;
    }

    void Start()
    {
        _ship = gameObject.GetComponent<Rigidbody>();

        transform.rotation = Quaternion.identity;
        _ship.velocity = transform.forward * defaultVelocity;
    }

    void FixedUpdate()
    {
        _verticalInput = Input.GetAxis("Vertical");

        if (!useAlternativeMouseInput)
        {
            _mouseInput = Input.mousePosition;

            float width = Screen.width * 0.5f;
            float height = Screen.height * 0.5f;

            _mouseInput.x = MapFloat(_mouseInput.x - width, -width, -1f, width, 1f);
            _mouseInput.y = MapFloat(_mouseInput.y - height, -height, -1f, height, 1f);
        }
        else
        {
            _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            Roll(-defaultRollingForce);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Roll(defaultRollingForce);
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


        Rotate(_mouseInput);

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
        float speedOffset = 1f;

        Vector3 velocity = direction * (_ship.velocity.magnitude);
        velocity += direction * force;

        float speed = velocity.magnitude;

        if (speed <= _actualMaxVelocity + speedOffset)
        {
            _ship.velocity = -velocity;
        }
        else
        {
            _ship.velocity = -transform.forward * maxVelocity;
        }
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

    /// <summary> 
    ///   <para> Boosts spaceship for a defined amount of time.</para>
    /// </summary>
    public void Boost()
    {
        if (_currentBoostTime < _maxBoostDuration)
        {
            _actualMaxVelocity =
                maxVelocity * _boostMultiplier; //set maximal velocity to default times boost multiplier
            Move(defaultVelocity * _boostMultiplier);
            _currentBoostTime++;
        }
    }
}