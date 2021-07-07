using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Laser Weapon
/// </summary>
public class LaserWeapon : Weapon
{
    private void Awake()
    {
        _weaponType = WeaponType.LASER;
    }

    protected override void FireProjectile()
    {
        GameObject firedMissleRight = Instantiate(_projectile, SocketRight.position, SocketRight.transform.rotation).gameObject;
        GameObject firedMissleLeft = Instantiate(_projectile, SocketLeft.position, SocketLeft.transform.rotation).gameObject;
        firedMissleRight.GetComponent<BlasterBehavior>().SetUp(_damage, Target, _projectileSpeed);
        firedMissleLeft.GetComponent<BlasterBehavior>().SetUp(_damage, Target, _projectileSpeed);
    }
}
