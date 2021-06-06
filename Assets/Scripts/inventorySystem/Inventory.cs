using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemPresents = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemPresents = true;
                }
            }
            if (!itemPresents)
            {
                itemList.Add(item);
            }
        } else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("Inventory item counter: " + itemList.Count);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
