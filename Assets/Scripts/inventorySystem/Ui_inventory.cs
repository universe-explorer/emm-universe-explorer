using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///   Represents Inventory window that gets notified each time when player colliders with
///   other Game Objects
/// </summary>
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

    /// <summary>
    ///   Sets the associated Inventory System which provides Events utilities and data accessibilities
    /// </summary>
    /// <param name="inventory">The Inventory on which UI operates on</param>
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemAdded += Inventory_OnItemAddeded;
        inventory.OnItemRemoved += Inventory_OnItemRemoved;
        RefreshInventoryItems();
    }

    /// <summary>
    ///   Refreshes Inventory Window on Items added
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Inventory_OnItemAddeded(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    /// <summary>
    ///  Refreshes Inventory Window on Items removed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Inventory_OnItemRemoved(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    /// <summary>
    ///   Sets the associated Game Object (Space ship) which provides location and other data 
    /// </summary>
    /// <param name="gameObject">Game Object on which UI operates on</param>
    public void SetGameObject(GameObject gameObject)
    {
        player = gameObject;
    }

    /// <summary> 
    ///   Updates Inventory UI Elements when the Items list is changed
    /// </summary>
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

            HandleMouseHover(itemSlotRectTransform, item);

            SetImage(itemSlotRectTransform, item);

            SetAmount(itemSlotRectTransform, item);
        }
    }

    /// <summary>
    ///   Handles mouse right click Events on the Inentory Item
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item"></param>
    private void HandleRightClick(Transform parent, Item item)
    {
        parent.GetComponent<MouseUIEvents>().RightClickHandler = () =>
        {
            Item duplicate = new Item {
                itemType = item.itemType,
                amount = item.amount
            };
            inventory.RemoveItem(item);
            ItemWorld.DropItem(player.transform.position, duplicate);
        };
    }

    /// <summary>
    ///   Handles mouse hover Events on the Inentory Item
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item"></param>
    private void HandleMouseHover(Transform parent, Item item)
    {
        TextMeshProUGUI infoText = parent.Find("info").GetComponent<TextMeshProUGUI>();
        infoText.SetText("");

        parent.GetComponent<MouseUIEvents>().MouseEnterHandler = () =>
        {
            infoText.SetText(item.GetTitle());
        };

        parent.GetComponent<MouseUIEvents>().MouseExitHandler = () =>
        {
            infoText.SetText("");
        };
    }

    /// <summary>
    ///   Sets Item Slot template image
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item"></param>
    private void SetImage(Transform parent, Item item)
    {
        Image image = parent.Find("image").GetComponent<Image>();
        image.sprite = item.GetSprite();
    }

    /// <summary>
    ///   Sets Item's amount
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item"></param>
    private void SetAmount(Transform parent, Item item)
    {
        TextMeshProUGUI amount = parent.Find("amount").GetComponent<TextMeshProUGUI>();
        amount.SetText(item.amount > 1 ? item.amount.ToString() : "");
    }
}
