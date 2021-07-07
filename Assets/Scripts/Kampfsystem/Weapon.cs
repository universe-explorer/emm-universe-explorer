using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Abstract Class that Represents a Weapon. Including Ammo, Weapon Type and FireRate
/// </summary>
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

    public string Name
    {
        get
        {
            if (_weaponType == WeaponType.LASER)
            {
                return "Laser Blaster";
            }
            else
            {
                return "Rocket Launcher";
            }
        }
    }
    
    [SerializeField]
    protected int _maxAmmo;

    protected bool _hasAmmo;

    protected bool _canShoot;

    protected WeaponType _weaponType;

    private int _currentAmmo;

    [SerializeField]
    private float _maxShootingDistance;

    [SerializeField]
    protected float _projectileSpeed;

    [SerializeField]
    protected Transform _projectile;

    [SerializeField]
    protected float _damage;

    [Header("Fire-Rate")]
    [SerializeField]
    private float _fireRate = 1f;

    public float FireRate
    {
        get
        {
            return _fireRate;
        }
    }

    [SerializeField]
    private float _nextFire;

    /// <summary>
    /// Position to spawn the Rocket at
    /// </summary>
    public Transform SocketLeft;
    public Transform SocketRight;

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

    /// <summary>
    /// Calls FireProjectile is the Weapon is active
    /// </summary>
    public void Fire()
    {
        if (_canShoot)
        {
            if (Time.time > _nextFire)
            {
                _nextFire = Time.time + _fireRate;

                FireProjectile();
                if (_hasAmmo)
                {
                    _currentAmmo--;
                }
            }
        }
        if (_maxAmmo <= 0 && _hasAmmo)
        {
            _canShoot = false;
        }
    }

    public float NextFire => _nextFire;

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

/// <summary>
/// Enum that stores the Information wether the Projectile should only tagret Enemies or Allies
/// </summary>
public enum Target
{
    Enemy, Allied
}
