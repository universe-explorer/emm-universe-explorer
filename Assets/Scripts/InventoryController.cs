using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Ui_inventory uiInventory;
    private Inventory inventory;

    /// <summary> 
    ///   Detects Collision and add items to the Inventory System
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "MainSpaceShip")
        {
            ItemWorld itemWorld = other.gameObject.GetComponent<ItemWorld>();
            if (itemWorld != null)
            {
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }
    }

    void Awake()
    {
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        uiInventory.SetGameObject(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            uiInventory.gameObject.SetActive(!uiInventory.gameObject.activeSelf);
        }
    }

    /// <summary> 
    ///   Returns the Inventory System attacted to the game object
    /// </summary>
    public Inventory GetInventory()
    {
        return inventory;
    }
}
