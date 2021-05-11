using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCombatController: MonoBehaviour
{
    public float _Health = 100f;
    public bool _CannShoot = true;
    protected float _MaxHealth;
    public float _MaxShootDistance = 200f;
    public int _Ammo;
    public float _Damage = 10f;

    public AbstractCombatController()
    {
        _MaxHealth = _Health;
    }
    public void TakeDamage(float amount)
    {
        _Health -= amount;
        if (_Health <= 0)
        {
            Debug.Log("Spaceship down!");
            Die();
        }
        HealthChanged();
    }

    public void GetHealth(float amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Eorror, amount below zero");
        }
        _Health += _Health;
        if (_Health > _MaxHealth)
        {
            _Health = _MaxHealth;
        }
        HealthChanged();
    }

    public abstract void Die();

    public abstract void HealthChanged();
}
