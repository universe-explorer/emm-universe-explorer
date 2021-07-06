using System;
using System.Collections;
using System.Collections.Generic;
using SerialCommunication;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    /* move */
    [Header("Movement Settings")] public bool enableDrifting = false;
    public bool autoDeceleration = false;
    private float defaultVelocity = 5;
    private float maxVelocity = 30;
    private float _accelerationSpeed = 0.3f;
    private float _deccelerationSpeed = 0.03f;

    /* rotate */
    [Header("Rotation Settings")] [SerializeField]
    private float sensitivity = 1;

    private float maxZRotation = 35;
    private float zRotationSpeed = 2.5f;

    /* boost */
    [Header("Boost Settings")] private float _boostMultiplier = 1.5f;
    private int _maxBoostDuration = 120;
    private bool _isBoosting;
    private int _currentBoostTime;

    /* rolling */
    [Header("Rolling Settings")] private float defaultRollingForce = 30f;

    private float _rollPerFrame = 14;
    private const float fullRoll = 360;
    private bool _startRoll;
    private bool _isRolling;
    private float _rollingDirection;


    /* input */
    [Header("Input Settings")] private float _verticalInput;
    private Vector2 _mouseInput;
    private Vector2 _mouseInputAngles;
    private Vector2 _mouseInputAnglesClamped;

    private JoystickReader _joystickReader;
    public bool useJoystick = false;


    /* crosshair */
    [Header("Crosshair Settings")] public Texture2D cursorTexture;
    private Vector2 _cursorOffset;

    /* other */
    private Rigidbody _ship;

    private InfoCircleScript _infoCircleScript;
    private SpeedDisplay _speedDisplay;

    [Header("AfterBurner")] [SerializeField]
    private ParticleSystem _afterBurner;

    [Header("FOV")] [SerializeField] private float _maxFOV = 80;

    [SerializeField] private float _minFOV = 50;

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

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, _cursorOffset, CursorMode.Auto);
    }


    void Start()
    {
        _cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        _ship = gameObject.GetComponent<Rigidbody>();

        _infoCircleScript = GetComponentInChildren<InfoCircleScript>();
        if (_infoCircleScript != null)
            _infoCircleScript.SetMaxValue(_maxBoostDuration);

        _speedDisplay = GetComponentInChildren<SpeedDisplay>();
        if (_speedDisplay != null)
        {
            _speedDisplay.SetNewMaxSpeed(maxVelocity * _boostMultiplier);
            _speedDisplay.SetNewCurrentSpeed(_ship.velocity.magnitude);
        }

        transform.rotation = Quaternion.identity;
        _ship.velocity = transform.forward * defaultVelocity;

        _joystickReader = JoystickReader.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !_isRolling)
        {
            _rollingDirection = 1;
            _startRoll = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !_isRolling)
        {
            _rollingDirection = -1;
            _startRoll = true;
        }
    }

    void FixedUpdate()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _isBoosting = false;

        if (_startRoll)
        {
            Roll(defaultRollingForce);
            _startRoll = false;
        }

        if (useJoystick)
        {
            Cursor.visible = false;
            _mouseInput = new Vector2((float) _joystickReader.pitch, (float) _joystickReader.roll);
        }
        else
        {
            Cursor.visible = true;
            _mouseInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isBoosting = true;
            Boost();
        }
        else
        {
            _isBoosting = false;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _minFOV, .1f);
            _afterBurner.Stop();

            if (_currentBoostTime > 0)
            {
                _currentBoostTime--;
            }
        }

        if (_infoCircleScript != null)
            _infoCircleScript.SetCurrentValue(_maxBoostDuration - _currentBoostTime);

        //Move if not rolling
        if (!_isRolling)
        {
            Move(_verticalInput);
            Rotate(_mouseInput);
        }

        if (_speedDisplay != null)
        {
            _speedDisplay.SetNewCurrentSpeed(_ship.velocity.magnitude);
        }
    }


    /// <summary> 
    ///     Applies force to the spaceship in the specified direction
    /// </summary>
    /// <param name="direction"> Movement direction</param>
    /// <param name="force"> Speed at which the spaceship should accelerate</param>
    public void Move(Vector3 direction, float force)
    {
        float _maxVelocity = maxVelocity;
        float speedOffset = .01f;


        if (autoDeceleration)
        {
            if (force == 0 && _ship.velocity.magnitude > 0)
            {
                _ship.velocity = Vector3.Lerp(_ship.velocity, -direction, _deccelerationSpeed);
            }
        }

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

        Vector3 newVelocity = Vector3.zero;
        
        //accelerate
        if (enableDrifting)
        {
            newVelocity = _ship.velocity + direction * force;
        }
        else
        {
            newVelocity = direction * (_ship.velocity.magnitude);
            newVelocity += direction * force;
        }

        if (newVelocity.magnitude + speedOffset <= _maxVelocity)
        {
            _ship.velocity = Vector3.Lerp(_ship.velocity, newVelocity, _accelerationSpeed);
        }
    }

    /// <summary> 
    ///     Applies force to move the spaceship forward
    /// </summary>
    /// <param name="force"> Speed at which the spaceship should accelerate</param>
    public void Move(float force)
    {
        Move(transform.forward, force);
    }

    /// <summary> 
    ///     Rotates the Spaceship according to the location of the mouse
    /// </summary>
    /// <param name="mouseInput"> Location of the mouse on scren</param>
    public void Rotate(Vector2 mouseInput)
    {
        mouseInput *= sensitivity;

        //Check if plane is upside down to reverse x Rotation direction
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            _mouseInputAngles.x -= mouseInput.x;
        }
        else
        {
            _mouseInputAngles.x += mouseInput.x;
        }

        _mouseInputAngles.y += mouseInput.y;


        //limit rotation to certain degree
        _mouseInputAnglesClamped.x += mouseInput.x;
        _mouseInputAnglesClamped.x = Mathf.Clamp(_mouseInputAnglesClamped.x, -maxZRotation, maxZRotation);

        //loop back to zero, used for spaceShip-wing rotation
        _mouseInputAnglesClamped.x = Mathf.Lerp(_mouseInputAnglesClamped.x, 0, Time.deltaTime * zRotationSpeed);

        _ship.rotation = Quaternion.Euler(_mouseInputAngles.y, _mouseInputAngles.x, -_mouseInputAnglesClamped.x);
    }

    /// <summary> 
    ///     Starts a roll with the specified force if a roll is not already in progress
    /// </summary>
    /// <param name="force"> Negative amount for left roll, positive amount for right roll</param>
    public void Roll(float force)
    {
        if (!_isRolling && force != 0)
        {
            _isRolling = true;

            if (force > 0)
            {
                StartCoroutine("RollCoroutine");
            }
            else
            {
                StartCoroutine("RollCoroutine");
            }

            Vector3 newMovement = transform.right * force;
            newMovement += transform.forward * Vector3.Dot(transform.forward, _ship.velocity);

            _ship.velocity = newMovement;
        }
    }

    IEnumerator RollCoroutine()
    {
        for (float i = 0; i < fullRoll; i += _rollPerFrame)
        {
            transform.Rotate(new Vector3(0, 0, _rollPerFrame * _rollingDirection), Space.Self);
            yield return null;
        }

        _isRolling = false;
    }

    /// <summary> 
    ///   Boosts spaceship as long as this method is called.
    /// </summary>
    public void Boost()
    {
        if (_currentBoostTime < _maxBoostDuration)
        {
            Move(defaultVelocity * _boostMultiplier);
            _currentBoostTime++;

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _maxFOV, .1f);
            if (_afterBurner.isPlaying == false)
            {
                _afterBurner.Play();
            }
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _minFOV, .1f);
            _afterBurner.Stop();
        }
    }

    /*
     * Getters and setters
     */

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
    ///   Sets a new maximum velocity
    /// </summary>
    /// <param name="newMaxVelocity"> New maximum velocity for the spaceship in Unity units</param>
    public void setMaximumVelocity(float newMaxVelocity)
    {
        maxVelocity = newMaxVelocity;
        if (_speedDisplay != null)
            _speedDisplay.SetNewMaxSpeed(maxVelocity * _boostMultiplier);
    }

    /// <summary> 
    ///   Returns the current boost multiplier
    /// </summary>
    public float getBoostMultiplier()
    {
        return _boostMultiplier;
    }

    /// <summary> 
    ///   Sets a new boost multiplier
    /// </summary>
    /// <param name="newBoostMultiplier"> New boost multiplier for the spaceship</param>
    public void setBoostMultiplier(float newBoostMultiplier)
    {
        _boostMultiplier = newBoostMultiplier;

        if (_speedDisplay != null)
            _speedDisplay.SetNewMaxSpeed(maxVelocity * _boostMultiplier);
    }

    /// <summary> 
    ///   Returns the current boost duration in frames
    /// </summary>
    public int getCurrentBoostDuration()
    {
        return _currentBoostTime;
    }

    /// <summary> 
    ///   Returns the maximum boost duration in frames
    /// </summary>
    public int getMaxBoostDuration()
    {
        return _maxBoostDuration;
    }

    /// <summary> 
    ///   Returns whether spaceship is currently rolling
    /// </summary>
    public bool getIsRolling()
    {
        return _isRolling;
    }

    /// <summary> 
    ///   Sets new maximum boost duration
    /// </summary>
    /// <param name="newBoostMultiplier"> New maximum boost duration for the spaceship in frames</param>
    public void setMaxBoostDuration(int newMaxBoostDuration)
    {
        _maxBoostDuration = newMaxBoostDuration;
        if (_infoCircleScript != null)
            _infoCircleScript.SetMaxValue(_maxBoostDuration);
    }


    /// <summary> 
    ///   Returns degrees of rotation per frame of rolling
    /// </summary>
    /// <param name="rollPerFrame"> Sets rolling steps per frame. I Roll = 360 degree</param>
    public void setRollPerFrame(float rollPerFrame)
    {
        _rollPerFrame = rollPerFrame;
    }

    /// <summary> 
    ///   Returns rolling steps per frame
    /// </summary>
    public float getRollPerFrame()
    {
        return _rollPerFrame;
    }
}