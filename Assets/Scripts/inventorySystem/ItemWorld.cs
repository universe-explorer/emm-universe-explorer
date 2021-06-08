using TMPro;
using UnityEngine; 

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        if (itemWorld == null) Debug.Log("SpawnItemWorld: itemWorld is null.");
        itemWorld.SetItem(item);
        Debug.Log("SpawnItemWorld: itemWorld " + itemWorld.GetItem().ToString() + "created.");

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
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

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    
}
