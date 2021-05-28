using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
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
        float _verticalInput = Input.GetAxis("Vertical");

        Vector3 _mousePosition = Input.mousePosition;
        Debug.Log(_mousePosition.x + " " + _mousePosition.y);
        

        if (Input.GetKeyDown(KeyCode.A))
        {
            Roll(defaultForce * -rollMultiplier);
        } else if(Input.GetKeyDown(KeyCode.D))
        {
            Roll(defaultForce * rollMultiplier);
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            Boost();
        } else if(_currentBoostTime > 0)
        {
            _currentBoostTime--;
        }

        if(_isRolling)
        {
            _currentRoll += rollPerFrame * _rollingDirection;
            Quaternion _shipRotation = transform.rotation;

            Vector3 rollDirection = new Vector3(0, 1, 0);

            transform.Rotate(new Vector3(0, 0, rollPerFrame * -_rollingDirection), Space.Self);

            if(_currentRoll == fullRoll || _currentRoll == -fullRoll)
            {
                _isRolling = false;
                _currentRoll = 0;
            }
        }

        Move(_verticalInput);

        Rotate(_mousePosition, 1);


        Debug.Log(-Vector3.Dot(_movementDirection, transform.forward));
    }


    /// <summary> 
    ///   <param name="direction"> Movement direction</param>
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
    public void Move(Vector3 direction, float force)
    {
        _movementDirection += direction * force;
        _ship.velocity = _movementDirection;
    }

    /// <summary> 
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
    public void Move(float force)
    {
        Move(transform.forward ,force);
    }

    /// <summary> 
    ///   <param name="direction"> Direction for the spaceship to rotate</param>
    ///   <param name="angle"> Rotation amount</param>
    /// </summary>
    public void Rotate(Vector3 direction, float angle)
    {

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        float xDifference = (direction.x - center.x) / 100;
        float yDifference = (direction.y - center.y) / 100;

        transform.Rotate(new Vector3(0, xDifference, 0));

        transform.Rotate(new Vector3(yDifference, 0, 0));
    }

    /// <summary> 
    ///   <param name="force"> Negative amount for left roll, positive amount for right roll</param>
    /// </summary>
    public void Roll(float force)
    {
        if(!_isRolling && force != 0)
        {
            if(force > 0)
            {
                _rollingDirection = 1;
            } else
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
        if(_currentBoostTime < _maxBoostDuration)
        {
            Move(defaultForce * _boostMultiplier);
            _currentBoostTime++;
        }
        
    }
    

}