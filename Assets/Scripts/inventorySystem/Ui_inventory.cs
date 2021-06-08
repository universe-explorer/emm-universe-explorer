using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui_inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private GameObject player;

    private void Start()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    public void SetGameObject(GameObject gameObject)
    {
        this.player = gameObject;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        if (itemSlotContainer == null) return;
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            HandleRightClick(itemSlotRectTransform, item);

            SetImage(itemSlotRectTransform, item);

            SetAmount(itemSlotRectTransform, item);
        }
    }

    private void HandleRightClick(Transform parent, Item item)
    {
        parent.GetComponent<MouseUIEvents>().RightClickHandler = () =>
        {
            Item duplicate = new Item { itemType = item.itemType, amount = item.amount };
            inventory.RemoveItem(item);
            ItemWorld.DropItem(player.transform.position, duplicate);
        };
    }

    private void SetImage(Transform parent, Item item)
    {
        Image image = parent.Find("image").GetComponent<Image>();
        image.sprite = item.GetSprite();
    }

    private void SetAmount(Transform parent, Item item)
    {
        TextMeshProUGUI amount = parent.Find("text").GetComponent<TextMeshProUGUI>();
        if (item.amount > 1)
        {
            amount.SetText(item.amount.ToString());
        }
        else
        {
            amount.SetText("");
        }
    }
}
