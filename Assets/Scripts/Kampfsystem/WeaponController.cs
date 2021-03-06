using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of all "installed" weapons and stores the information which weapon is active
/// </summary>
public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private KeyCode _key;

    private int _activeWeaponIndex = 0;

    private void Start()
    {
        _activeWeapon = Weapons[_activeWeaponIndex];
    }

    [SerializeField]
    private float _damageMultiplier = 1f;

    public float DamageMultiplier
    {
        get
        {
            return _damageMultiplier;
        }
        set
        {
            _damageMultiplier = value;
        }
    }

    private Weapon _activeWeapon;

    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            SwitchWeapon();
        }
    }

    /// <summary>
    /// Fires the weapon that is currently active
    /// </summary>
    public void FireActiveWeapon()
    {
        _activeWeapon.Fire();
    }

    /// <summary>
    /// Switches the Weapon to the next, in order, not randomly
    /// </summary>
    public void SwitchWeapon()
    {
        if (Weapons.Length <= 1)
        {
            return;
        }
        //Debug.Log("Current Weapon: " + _activeWeaponIndex);
        int newWeaponIndex = _activeWeaponIndex;
        while (_activeWeapon == Weapons[newWeaponIndex])
        {
            newWeaponIndex++;
            if (newWeaponIndex >= Weapons.Length)
            {
                newWeaponIndex = 0;
            }
        }
        _activeWeapon = Weapons[newWeaponIndex];
        _activeWeaponIndex = newWeaponIndex;
        //Debug.Log("New Weapon: " + _activeWeaponIndex);
    }

    [SerializeField]
    public Weapon[] Weapons;

    
    public int ActiveWeaponIndex => _activeWeaponIndex;

    /// <summary>
    /// Switches to the installed weapon with index
    /// </summary>
    /// <param name="newWeaponIndex">the Weapon index to switch to</param>
    /// <returns>true if weapon was switched, false if the weapon index is greater than the installed weapons count</returns>
    public bool SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex < Weapons.Length)
        {
            _activeWeaponIndex = newWeaponIndex;
            _activeWeapon = Weapons[newWeaponIndex];
            return true;
        }
        return false;
    }
}
