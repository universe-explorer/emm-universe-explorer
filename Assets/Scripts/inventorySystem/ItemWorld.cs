using TMPro;
using UnityEngine; 

/// <summary>
///   Utility to spawn Item with the given position, also provides the ability
///   to respawn Items inside Game World after being removeed from the Inventory
/// </summary>
public class ItemWorld : MonoBehaviour
{
    /// <summary>
    ///   Spawns a Item World and sets its Item
    /// </summary>
    /// <param name="position">Position where the Item is to be spawned</param>
    /// <param name="item">Item</param>
    /// <returns>ItemWorld</returns>
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    /// <summary>
    ///   Detaches the associated Item of the Item World and respawns the Item in the game world
    /// </summary>
    /// <param name="dropPosition">Position where the Item is to be respawned</param>
    /// <param name="item">Item</param>
    /// <returns>ItemWorld</returns>
    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 10f, item);
        itemWorld.GetComponent<Rigidbody>().AddForce(randomDir * 10f, ForceMode.Impulse);
        return itemWorld;
    }

    private Item item;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro amount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        amount = transform.Find("text").GetComponent<TextMeshPro>();
    }

    /// <summary>
    ///   Sets the Item of this Item World
    /// </summary>
    /// <param name="item">Item</param>
    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1)
        {
            amount.SetText(item.amount.ToString());
        } else
        {
            amount.SetText("");
        }
    }

    /// <summary> 
    ///   Returns the associated Item with this ItemWorld 
    /// </summary>
    public Item GetItem()
    {
        return item;
    }

    /// <summary> 
    ///   Destroys this ItemWorld
    /// </summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    
}
