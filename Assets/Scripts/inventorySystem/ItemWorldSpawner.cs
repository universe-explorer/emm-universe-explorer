using UnityEngine;

/// <summary> 
///   Provides the ability to easily spawn items inside Game World,
///   so there is no need to manuelly set item's positions
/// </summary>
public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;

    /// <summary>
    ///   
    /// </summary>
    private void Start()
    {
        ItemWorld.SpawnItemWorld(transform.position, item);
        Destroy(gameObject);
    }
}
