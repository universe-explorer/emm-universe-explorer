using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private void Start()
    {
        _activeWeapon = Weapons[0];
    }

    private Weapon _activeWeapon;



    public void FireActiveWeapon()
    {
        _activeWeapon.Fire();
    }

    public void SwitchWeapon()
    {

    }

    [SerializeField]
    public Weapon[] Weapons;
}
