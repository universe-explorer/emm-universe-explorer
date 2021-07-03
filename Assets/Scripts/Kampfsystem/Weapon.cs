using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    public Target Target;

    public int MaxAmmo
    {
        get
        {
            return _maxAmmo;
        }
        protected set
        {
            _maxAmmo = value;
        }
    }

    public int CurrentAmmo
    {
        get
        {
            return _currentAmmo;
        }
        protected set
        {
            _currentAmmo = value;
        }
    }

    public WeaponType WeaponType 
    {
        get
        {
            return _weaponType;
        }
        protected set
        {
            _weaponType = value;
        }
    }
    
    [SerializeField]
    protected int _maxAmmo;

    protected bool _hasAmmo;

    protected bool _canShoot;

    [SerializeField]
    protected WeaponType _weaponType;

    private int _currentAmmo;

    [SerializeField]
    private float _maxShootingDistance;
    
    public void Start()
    {
        if (_maxAmmo < 0)
        {
            _maxAmmo = 0;
        }

        if (_maxAmmo > 0)
        {
            _hasAmmo = true;
            _currentAmmo = _maxAmmo;
            _canShoot = true;
        }
        else
        {
            _hasAmmo = false;
            _maxAmmo = 0;
            _currentAmmo = 0;
            _canShoot = true;
        }

    }

    public void Fire()
    {
        if (_canShoot)
        {
            FireProjectile();
            if (_hasAmmo)
            {
                _currentAmmo--;
            }
        }
        if (_maxAmmo <= 0 && _hasAmmo)
        {
            _canShoot = false;
        }
    }

    protected abstract void FireProjectile();

    public void Reload()
    {
        if (_hasAmmo == false)
        {
            return;
        }

        _currentAmmo = _maxAmmo;
        _canShoot = true;
    }
}

public enum Target
{
    Enemy, Allied
}
