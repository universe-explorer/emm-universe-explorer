using System.Collections;
using System.Collections.Generic;
using SerialCommunication;
using UnityEngine;

/// <summary>
/// Combat Controller that belongs to an Allie
/// </summary>
public class CombatControllerPlayer : AbstractCombatController
{
    public LineRenderer _LineRenderer;

    public float _LaserWidth = 0.1f;
    public float _LaserMaxLength;

    [SerializeField]
    public WeaponController WeaponController;

    private HealthBarScript _healthBarScript;
    private JoystickReader _joystickReader;
    
    void Start()
    {
        if (WeaponController == null)
        {
            WeaponController = gameObject.GetComponentInChildren<WeaponController>();
        }

        _healthBarScript = gameObject.GetComponentInChildren<HealthBarScript>();
        
        _joystickReader = JoystickReader.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_CannShoot == true)
        {
            if (Input.GetButtonDown("Fire1") || _joystickReader.btn_state)
            {
                WeaponController.FireActiveWeapon();
            }
        }
    }
    
    public override void Die()
    {
        GetComponentInChildren<DeathScreenManager>().enableDeathScreen(); // TODO: test before merging with main
        //throw new System.NotImplementedException();
    }

    public override void HealthChanged()
    {
        _healthBarScript.TakeDamageTemporary(_Health); // Test implementation -> health can also increase in this method
        //throw new System.NotImplementedException();
    }
}
