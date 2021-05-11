using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerEnemy : AbstractCombatController
{
    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void HealthChanged()
    {
        Debug.Log("Enemy HP: " + _Health);
    }
}
