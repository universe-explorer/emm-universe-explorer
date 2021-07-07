using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/// <summary>
/// The MeteorSPawnerField is Isntanciated at runtime and Spawnes Collectables or Enemys when the Trigger is entered.
/// </summary>
[ExecuteInEditMode]
public class MeteorSpawnerField : MonoBehaviour
{
    private Vector3Int _iD;

    [Header("MeteoridField Settings")]
    private Bounds bounds;

    public float ColliderSize { get { return _colliderOffest;  } }

    private BoxCollider _boxCollider;

    [HideInInspector]
    public bool SettingsFoldout = false;

    [SerializeField]
    private float _colliderOffest = 5f;

    public AsteroidSettings AsteroidSettings;

    [SerializeField] private VisualEffectAsset itemVfxAsset;
    [SerializeField] private Gradient _gradientHealth, _gradientMana, _gradientMineral, _gradientMedikit;
    
    private GameObject enemyPrefab;
    private GameObject enemyPrefabRare;
    private Object[] scriptableObjects;

    public void Start()
    {
        enemyPrefab = (GameObject)Resources.Load("Enemy", typeof(GameObject));
        enemyPrefabRare = (GameObject)Resources.Load("Rare Enemy", typeof(GameObject));
        scriptableObjects = Resources.LoadAll("ScriptableObjects", typeof(EnemyScriptableObject));
    }

    private void SpawnMeteors()
    {
        float asteroidScale;
        for (int i = 0; i < MeteorSpawnerFieldCreator.Instance.Meteors.Length; i++)
        {
            for (int j = 0; j < (AsteroidSettings.normalSizedAsteroids + AsteroidSettings.hugeSizedAsteroids) / MeteorSpawnerFieldCreator.Instance.Meteors.Length; j++)
            {
                asteroidScale = Random.Range(AsteroidSettings.hugeAsteroidSizeMin, AsteroidSettings.hugeAsteroidSizeMax);

                int randomMeteor = Random.Range(0, MeteorSpawnerFieldCreator.Instance.Meteors.Length);

                Transform spawnedAsteroid = Instantiate(MeteorSpawnerFieldCreator.Instance.Meteors[randomMeteor], RandomPointInBounds(bounds), Quaternion.identity, gameObject.transform);
                spawnedAsteroid.gameObject.AddComponent<AsteroidBehaviour>().Setup(Random.Range(AsteroidSettings.thrustMin, AsteroidSettings.thrustMax), Random.Range(AsteroidSettings.rotationSpeedMin, AsteroidSettings.rotationSpeedMax), AsteroidSettings._mass, AsteroidSettings._drag, AsteroidSettings._angularDrag);
                spawnedAsteroid.transform.localScale *= asteroidScale;
            }
        }
    }

    private void SpawnCollectables()
    {
        int numberOfCollectables = (int) Random.Range(0, 3);

        for(int i = 0; i < numberOfCollectables; i++)
        {
            Item.ItemType itemType = (Item.ItemType)Random.Range(0, System.Enum.GetNames(typeof(Item.ItemType)).Length);

            Item item = new Item();
            item.itemType = itemType;
            item.amount = Random.Range(1, 5);
            
            ItemWorld itemWorld = ItemWorld.SpawnItemWorld(RandomPointInBounds(bounds), item);
            itemWorld.transform.SetParent(transform);
            

            VisualEffect vfx = itemWorld.AddComponent<VisualEffect>();
            List<VFXExposedProperty> exposedProperties = new List<VFXExposedProperty>();
            itemVfxAsset.GetExposedProperties(exposedProperties);
            //exposedProperties.ForEach(p => Debug.Log(p.name));
            //VFXExposedProperty glowColorProperty = exposedProperties.Find(new Predicate<VFXExposedProperty>(p => p.name == "GlowColor"));
            
            
            vfx.visualEffectAsset = itemVfxAsset;
            Gradient g;
            switch (item.itemType)
            {
                case Item.ItemType.Health:
                    g = _gradientHealth;
                    break;
                case Item.ItemType.Mana:
                    g = _gradientMana;
                    break;
                case Item.ItemType.Mineral:
                    g = _gradientMineral;
                    break;
                case Item.ItemType.Medkit:
                    g = _gradientMedikit;
                    break;
                default:
                    g = new Gradient();
                    break;
                    
            }
            vfx.SetGradient("GlowColor", g);
            

        }
    }

    private void SpawnEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length <= 3)
        {
            int randomIndex = (int)Random.Range(0, scriptableObjects.Length);
            EnemyScriptableObject enemyScriptableObject = (EnemyScriptableObject)scriptableObjects[randomIndex];

            EnemyBehaviour behaviour = (EnemyBehaviour)enemyPrefab.GetComponent(typeof(EnemyBehaviour));
            behaviour.values = enemyScriptableObject;
            int r = Random.Range(1, 6);
            if (r == 1)
            {
                Instantiate(enemyPrefabRare, RandomPointInBounds(bounds), Quaternion.identity, gameObject.transform);

            }
            else
            {
                Instantiate(enemyPrefab, RandomPointInBounds(bounds), Quaternion.identity, gameObject.transform);
            }
        }
    }

    /// <summary> 
    ///     Adds BoxCollider component if it doesn't already exist
    /// </summary>
    public void CreateCollider()
    {
        if (gameObject.GetComponent<BoxCollider>() == null)
        {
            _boxCollider = gameObject.AddComponent<BoxCollider>();
        }
        else
        {
            _boxCollider = gameObject.GetComponent<BoxCollider>();
        }
        
        _boxCollider.size = Vector3.one *  _colliderOffest;
        _boxCollider.isTrigger = true;

        bounds = _boxCollider.bounds;
    }

    /// <summary> 
    ///     Generates a random coordinate within given bounds
    ///   <param name="bounds"> The bounds to generate a coordinate in</param>
    /// </summary>
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
            );
    }

    private void OnValidate()
    {
        CreateCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            SpawnMeteors();
            SpawnCollectables();
            SpawnEnemies();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            DestroyMeteors();
            DestroyEnemies();
            DestroyItems();
        }
    }

    private void DestroyMeteors()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AsteroidBehaviour behaviour = transform.GetChild(i).gameObject.GetComponent<AsteroidBehaviour>();
            if(behaviour != null)
            {
                behaviour.Remove();
            }
        }
        
    }

    private void DestroyEnemies()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CombatControllerEnemy behaviour = transform.GetChild(i).gameObject.GetComponent<CombatControllerEnemy>();
            if (behaviour != null)
            {
                behaviour.Unload();
            }
        }

    }

    private void DestroyItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ItemWorld behaviour = transform.GetChild(i).gameObject.GetComponent<ItemWorld>();
            if (behaviour != null)
            {
                behaviour.DestroySelf();
            }
        }

    }

}