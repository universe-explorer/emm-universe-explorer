using UnityEngine;

/*
 * provide ability to easily spawn items, so there is no need to manuelly set
 * item positions
 */
public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;

    private void Start()
    {
        ItemWorld.SpawnItemWorld(transform.position, item);
        Destroy(gameObject);
    }
}
