using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherWeapon : Weapon
{
    [SerializeField]
    private float _rocketSpeed = 5f;

    [SerializeField]
    private Transform _rocket;

    /// <summary>
    /// Position to spawn the Rocket at
    /// </summary>
    public Transform SocketLeft;
    public Transform SocketRight;

    protected override void FireProjectile()
    {
        GameObject firedMissleRight = Instantiate(_rocket, SocketRight.position, SocketRight.transform.rotation).gameObject;
        GameObject firedMissleLeft = Instantiate(_rocket, SocketLeft.position, SocketLeft.transform.rotation).gameObject;
        //firedMissle.GetComponent<RocketBehaivor>().SetUp();
        firedMissleRight.GetComponent<RocketBehaivor>().Engage(_rocketSpeed);
        firedMissleLeft.GetComponent<RocketBehaivor>().Engage(_rocketSpeed);
        //LaunchRocket(firedMissle);
        Destroy(firedMissleRight, 15f);
        Destroy(firedMissleLeft, 15f);
    }

    IEnumerator LaunchRocket(GameObject rocket)
    {
        yield return new WaitForSeconds(10f);
        Destroy(rocket);
        Debug.Log("Rocket Destroyed");
        yield return null;
    }
    
}
