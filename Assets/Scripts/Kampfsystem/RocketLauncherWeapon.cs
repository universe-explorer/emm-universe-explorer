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
    public Transform Socket;

    protected override void FireProjectile()
    {
        GameObject firedMissle = Instantiate(_rocket, Socket.position, Socket.transform.rotation).gameObject;
        //firedMissle.GetComponent<RocketBehaivor>().SetUp();
        firedMissle.GetComponent<RocketBehaivor>().Engage();
        //LaunchRocket(firedMissle);
        Destroy(firedMissle, 15f);
    }

    IEnumerator LaunchRocket(GameObject rocket)
    {
        yield return new WaitForSeconds(10f);
        Destroy(rocket);
        Debug.Log("Rocket Destroyed");
        yield return null;
    }
    
}
