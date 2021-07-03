using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class RocketLauncherWeapon : Weapon
{
    [SerializeField]
    private float _rocketSpeed = 5f;

    [SerializeField]
    private Transform _rocket;

    [SerializeField]
    private float _damage = 10f;

    /// <summary>
    /// Position to spawn the Rocket at
    /// </summary>
    public Transform SocketLeft;
    public Transform SocketRight;

    protected override void FireProjectile()
    {
        GameObject firedMissleRight = Instantiate(_rocket, SocketRight.position, SocketRight.transform.rotation).gameObject;
        GameObject firedMissleLeft = Instantiate(_rocket, SocketLeft.position, SocketLeft.transform.rotation).gameObject;
        firedMissleRight.GetComponent<RocketBehaivor>().SetUp(_damage, Target);
        firedMissleLeft.GetComponent<RocketBehaivor>().SetUp(_damage, Target);
        firedMissleRight.GetComponent<RocketBehaivor>().Engage(_rocketSpeed);
        firedMissleLeft.GetComponent<RocketBehaivor>().Engage(_rocketSpeed);
        //LaunchRocket(firedMissle);
        //Destroy(firedMissleRight, 15f);
        //Destroy(firedMissleLeft, 15f);
    }
}
