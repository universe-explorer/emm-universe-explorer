using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Rocket Launcher Weapon
/// </summary>
public class RocketLauncherWeapon : Weapon
{
    private void Awake()
    {
        _weaponType = WeaponType.ROCKET;
    }
    protected override void FireProjectile()
    {
        GameObject firedMissleRight = Instantiate(_projectile, SocketRight.position, SocketRight.transform.rotation).gameObject;
        GameObject firedMissleLeft = Instantiate(_projectile, SocketLeft.position, SocketLeft.transform.rotation).gameObject;
        firedMissleRight.GetComponent<RocketBehaivor>().SetUp(_damage, Target);
        firedMissleLeft.GetComponent<RocketBehaivor>().SetUp(_damage, Target);
        firedMissleRight.GetComponent<RocketBehaivor>().Engage(_projectileSpeed);
        firedMissleLeft.GetComponent<RocketBehaivor>().Engage(_projectileSpeed);
        //LaunchRocket(firedMissle);
        //Destroy(firedMissleRight, 15f);
        //Destroy(firedMissleLeft, 15f);
    }
}
