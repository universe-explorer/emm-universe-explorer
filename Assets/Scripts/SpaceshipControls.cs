using System;
using UnityEngine;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    /* move */
    private const float defaultVelocity = 5;
    private float maxVelocity = 50;
    private float _accelerationSpeed = 0.1f;


    /* rotate */
    private const float sensitivity = 1;
    private const float maxZRotation = 35;
    private const float zRotationSpeed = 2.5f;


    /* boost */
    private float _boostMultiplier = 1.5f;
    private int _maxBoostDuration = 120;
    private bool _isBoosting;
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
        float t = Mathf.InverseLerp(fromMin, fromMax, value);
        return Mathf.Lerp(toMin, toMax, t);
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
        _isBoosting = false;
        if (!useAlternativeMouseInput)
        {
            _mouseInput = Input.mousePosition;

            float width = Screen.width * 0.5f;
            float height = Screen.height * 0.5f;

            _mouseInput.x = MapFloat(_mouseInput.x - width, -width, width, -1.0f, 1.0f);
            _mouseInput.y = -MapFloat(_mouseInput.y - height, -height, height, -1.0f, 1.0f);
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
            _isBoosting = true;
            Boost();
        } else
        {
            _isBoosting = false;

            if(_currentBoostTime > 0)
            {
                _currentBoostTime--;
            }
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
        } else
        {
            Move(_verticalInput);
        }

        Rotate(_mouseInput);

    }


    /// <summary> 
    ///   <param name="direction"> Movement direction</param>
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
    public void Move(Vector3 direction, float force)
    {
        float _maxVelocity = maxVelocity;
        float speedOffset = .01f;

        if (_isBoosting && _currentBoostTime < _maxBoostDuration)
        {
            _maxVelocity *= _boostMultiplier;
        }

        //if speed before accelleration > _maxVelocity
        if (_ship.velocity.magnitude + speedOffset > _maxVelocity)
        {
            _ship.velocity = Vector3.Lerp(_ship.velocity, direction * maxVelocity, _accelerationSpeed);
            return;
        }


        //accelerate
        Vector3 newVelocity = direction * (_ship.velocity.magnitude);
        newVelocity += direction * force;

        if (newVelocity.magnitude + speedOffset <= _maxVelocity)
        {
            _ship.velocity = Vector3.Lerp(_ship.velocity, newVelocity, _accelerationSpeed);
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
    ///   <param name="mouseInput"> Location of the mouse on scren</param>
    /// </summary>
    public void Rotate(Vector2 mouseInput)
    {
        mouseInput *= sensitivity;
        _mouseInputAngles.x += mouseInput.x;
        _mouseInputAngles.y += mouseInput.y;


        //limit rotation to certain degree
        _mouseInputAnglesClamped.x += mouseInput.x;
        _mouseInputAnglesClamped.x = Mathf.Clamp(_mouseInputAnglesClamped.x, -maxZRotation, maxZRotation);

        //loop back to zero, used for spaceShip-wing rotation
        _mouseInputAnglesClamped.x = Mathf.Lerp(_mouseInputAnglesClamped.x, 0, Time.deltaTime * zRotationSpeed);


        _ship.rotation = Quaternion.Euler(_mouseInputAngles.y, _mouseInputAngles.x, -_mouseInputAnglesClamped.x);
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
    ///   Boosts spaceship for a defined amount of time.
    /// </summary>
    public void Boost()
    {
        if (_currentBoostTime < _maxBoostDuration)
        {
            Move(defaultVelocity * _boostMultiplier);
            _currentBoostTime++;
        }
    }


    //Getters and setters

    /// <summary> 
    ///   Returns current movement as a vector.
    /// </summary>
    public Vector3 getCurrentMovement()
    {
        return _ship.velocity;
    }

    /// <summary> 
    ///   Returns maximum velocity in Unity units
    /// </summary>
    public float getMaximumVelocity()
    {
        return maxVelocity;
    }


    /// <summary> 
    ///   <param name="newMaxVelocity"> New maximum velocity for the spaceship in Unity units</param>
    /// </summary>
    public void setMaximumVelocity(float newMaxVelocity)
    {
        maxVelocity = newMaxVelocity;
    }

    /// <summary> 
    ///   Returns the current boost multiplier
    /// </summary>
    public float getBoostMultiplier()
    {
        return _boostMultiplier;
    }

    /// <summary> 
    ///   <param name="newBoostMultiplier"> New boost multiplier for the spaceship</param>
    /// </summary>
    public void setBoostMultiplier(float newBoostMultiplier)
    {
        _boostMultiplier = newBoostMultiplier;
    }

    /// <summary> 
    ///   Returns the current maximum boost duration in frames
    /// </summary>
    public int getMaxBoostDuration()
    {
        return _maxBoostDuration;
    }

    /// <summary> 
    ///   <param name="newBoostMultiplier"> New maximum boost duration for the spaceship in frames</param>
    /// </summary>
    public void setMaxBoostDuration(int newMaxBoostDuration)
    {
        _maxBoostDuration = newMaxBoostDuration;
    }
}