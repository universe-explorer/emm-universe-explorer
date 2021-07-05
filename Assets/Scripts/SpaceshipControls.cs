using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    /* move */
    [Header("Movement Settings")] public bool enableDrifting = true;
    public bool autoDeceleration = false;

    [SerializeField] //remove in production
    private float defaultVelocity = 5;

    [SerializeField] //remove in production
    private float maxVelocity = 30;

    [SerializeField] //remove in production
    private float _accelerationSpeed = 0.3f;

    [SerializeField] //remove in production
    private float _deccelerationSpeed = 0.03f;


    /* rotate */
    [Header("Rotation Settings")] [SerializeField]
    private float sensitivity = 1;

    [SerializeField] //remove in production
    private float maxZRotation = 35;

    [SerializeField] //remove in production
    private float zRotationSpeed = 2.5f;


    /* boost */
    [Header("Boost Settings")] [SerializeField] //remove in production
    private float _boostMultiplier = 1.5f;

    [SerializeField] //remove in production
    private int _maxBoostDuration = 120;

    private bool _isBoosting;
    private int _currentBoostTime;


    /* rolling */
    [Header("Rolling Settings")] [SerializeField] //remove in production
    private float defaultRollingForce = 30f;

    [SerializeField][Range(4.0f, 80.0f)] private float _rollPerFrame = 15;

    private const float fullRoll = 360;
    private bool _startRoll;
    private bool _isRolling;
    private float _rollingDirection;


    /* input */
    [Header("Input Settings")] public bool useAlternativeMouseInput = false;

    private float _verticalInput;
    private Vector2 _mouseInput;
    private Vector2 _mouseInputAngles;
    private Vector2 _mouseInputAnglesClamped;


    /* crosshair */
    [Header("Crosshair Settings")] [SerializeField] //remove in production
    private Boolean _debugCrosshair = false;

    private GameObject _crosshair, _crosshairUI;

    [SerializeField] //remove in production
    private float _crosshairOffsetX = 18f;

    [SerializeField] //remove in production
    private float _crosshairOffsetY = 7f;

    [SerializeField] //remove in production
    private float _crosshairMovementSpeed = 100f;

    private Vector3 _crosshairPosition;

    /* other */
    private Rigidbody _ship;

    [Header("Debug")] public float velocity;

    private InfoCircleScript _infoCircleScript;
    private SpeedDisplay _speedDisplay;

    [Header("AfterBurner")]
    [SerializeField]
    private ParticleSystem _afterBurner;

    [Header("FOV")]
    [SerializeField]
    private float _maxFOV = 80;

    [SerializeField]
    private float _minFOV = 50;

    [SerializeField]
    [Range(0, 1)]
    private float _amount = .1f;

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
        _crosshair = GameObject.FindGameObjectsWithTag("Crosshair")[0];
        _crosshairUI = GameObject.FindGameObjectsWithTag("CrosshairUI")[0];

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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
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
        velocity = _ship.velocity.magnitude;
        _verticalInput = Input.GetAxis("Vertical");
        _isBoosting = false;

        if (_startRoll)
        {
            Roll(defaultRollingForce);
            _startRoll = false;
        }

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
            SetCrosshairPosition(_mouseInput);
        }

        if (_speedDisplay != null)
        {
            _speedDisplay.SetNewCurrentSpeed(_ship.velocity.magnitude);
        }
    }


    /// <summary> 
    ///   <param name="direction"> Movement direction</param>
    ///   <param name="force"> Speed at which the spaceship should accelerate</param>
    /// </summary>
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

        //Check if plane is upside down to reverse x Rotation direction
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            _mouseInputAngles.x -= mouseInput.x;
        } else
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
    ///   Boosts spaceship for a defined amount of time.
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
        } else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _minFOV, .1f);
            _afterBurner.Stop();
        }
    }

    /// <summary> 
    ///   <para>Places the crosshair relative to mouse position</para>
    /// </summary>
    public void SetCrosshairPosition(Vector2 mouseInput)
    {
        Vector3 pos = _crosshair.transform.localPosition;

        float x = mouseInput.x * _crosshairOffsetX;
        float y = mouseInput.y * _crosshairOffsetY * (-1);

        x = Mathf.Lerp(pos.x, x, Time.deltaTime * _crosshairMovementSpeed);
        y = Mathf.Lerp(pos.y, y, Time.deltaTime * _crosshairMovementSpeed);

        _crosshairPosition = _ship.transform.position
                             + _ship.transform.forward * 10
                             + _ship.transform.right * x
                             + _ship.transform.up * y;

        _crosshair.transform.position = _crosshairPosition;

        //convert world coordinates to 2d position
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(_crosshairPosition);
        _crosshairUI.transform.position =
            Vector3.Lerp(_crosshairUI.transform.position, screenPoint, Time.deltaTime * 25);

        //debug options
        if (_debugCrosshair)
        {
            _crosshair.GetComponent<MeshRenderer>().enabled = true;
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
        if(_speedDisplay != null)
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
    ///   <param name="newBoostMultiplier"> New boost multiplier for the spaceship</param>
    /// </summary>
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
    ///   Returns if spaceship is currently rolling
    /// </summary>
    public bool getIsRolling()
    {
        return _isRolling;
    }

    /// <summary> 
    ///   <param name="newBoostMultiplier"> New maximum boost duration for the spaceship in frames</param>
    /// </summary>
    public void setMaxBoostDuration(int newMaxBoostDuration)
    {
        _maxBoostDuration = newMaxBoostDuration;
        if (_infoCircleScript != null)
            _infoCircleScript.SetMaxValue(_maxBoostDuration);
    }

    /// <summary> 
    ///   <param> Returns crosshair aiming direction</param>
    /// </summary>
    public Vector3 getShootingDirection()
    {
        //(to - from).normalized
        return (_crosshairPosition - _ship.transform.position).normalized; //todo: is this right?
    }
    
    
    /// <summary> 
    ///   <param name="rollPerFrame"> Sets rolling steps per frame. I Roll = 360 degree</param>
    /// </summary>
    public void setRollPerFrame(float rollPerFrame)
    {
        _rollPerFrame = rollPerFrame;
    }

    /// <summary> 
    ///   <param> Returns rolling steps per frame</param>
    /// </summary>
    public float getRollPerFrame()
    {
        return _rollPerFrame;
    }
}