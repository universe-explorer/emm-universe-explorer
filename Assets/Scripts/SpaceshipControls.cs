using System;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipControls : MonoBehaviour, ISpaceshipControls
{
    /* move */
    public bool enableDrifting = true;
    private const float defaultVelocity = 5;
    private float maxVelocity = 30;
    private float _accelerationSpeed = 0.3f;


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
    private const float defaultRollingForce = 30f;
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


    /* crosshair */
    public Boolean debugCrosshair = false;

    private GameObject _crosshair, _crosshairUI;
    private float _crosshairOffset = 1.5f;
    private float _crosshairMovementSpeed = 2.5f;
    private Vector3 _crosshairPosition;

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
    
    [SerializeField] private Ui_inventory uiInventory;
    [SerializeField] private Ui_level uiLevel;

    private Inventory inventory;
    private LevelSystem levelSystem;

    /// <summary> 
    ///   Detects Collision and add items to the Inventory System
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "MainSpaceShip")
        {
            ItemWorld itemWorld = other.gameObject.GetComponent<ItemWorld>();
            if (itemWorld != null)
            {
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }
    }

    private void Awake()
    {
        // initialize Inventory System
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        uiInventory.SetGameObject(gameObject);

        // initialize Level System
        levelSystem = new LevelSystem();
        levelSystem.SetInventory(inventory);
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
        uiLevel.SetLevelSystem(levelSystem);
    }

    /// <summary> 
    ///   Updates Player's Properties based on the current level
    /// </summary>
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        PlayerRankEntry entry = levelSystem.GetCurrentPlayerLevelRank();
        setMaximumVelocity(entry.MaxVelocity);
        setMaxBoostDuration(entry.BoostDuration);
    }

    void Start()
    {
        _ship = gameObject.GetComponent<Rigidbody>();
        _crosshair = GameObject.FindGameObjectsWithTag("Crosshair")[0];
        _crosshairUI = GameObject.FindGameObjectsWithTag("CrosshairUI")[0];

        transform.rotation = Quaternion.identity;
        _ship.velocity = transform.forward * defaultVelocity;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiInventory.gameObject.SetActive(!uiInventory.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            uiLevel.gameObject.SetActive(!uiLevel.gameObject.activeSelf);
        }
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
        }
        else
        {
            _isBoosting = false;

            if (_currentBoostTime > 0)
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
        }
        else
        {
            Move(_verticalInput);
            Rotate(_mouseInput);
            SetCrosshairPosition(_mouseInput);
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
                _rollingDirection = -1;
            }
            else
            {
                _rollingDirection = 1;
            }

            Vector3 newMovement = transform.right * force;
            newMovement += transform.forward * Vector3.Dot(transform.forward, _ship.velocity);

            _ship.velocity = newMovement;

            //Move(transform.right * _rollingDirection, Math.Abs(force));
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

    /// <summary> 
    ///   <para>Places the crosshair relative to mouse position</para>
    /// </summary>
    public void SetCrosshairPosition(Vector2 mouseInput)
    {
        Vector3 pos = _crosshair.transform.localPosition;

        float x = mouseInput.x * _crosshairOffset;
        float y = mouseInput.y * _crosshairOffset * (-1);

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
            Vector3.Lerp(_crosshairUI.transform.position, screenPoint, Time.deltaTime * 10f);

        //debug options
        if (debugCrosshair)
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

    /// <summary> 
    ///   <param> Returns crosshair aiming direction</param>
    /// </summary>
    public Vector3 getShootingDirection()
    {
        //(to - from).normalized
        return (_crosshairPosition - _ship.transform.position).normalized; //todo: is this right?
    }
}