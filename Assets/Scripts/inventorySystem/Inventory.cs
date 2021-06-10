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

    /// <summary> 
    ///   Adds item to the item list
    /// </summary>
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
        Debug.Log("Inventory item counter: " + GetTotalItemsCount().ToString());
    }

    /// <summary> 
    ///   Removes item from the item list
    /// </summary>
    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item found = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    found = inventoryItem;
                }
            }
            if (found != null && found.amount <= 0)
            {
                itemList.Remove(found);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("Inventory item counter: " + GetTotalItemsCount());
    }

    /// <summary> 
    ///   Returns the item list
    /// </summary>
    public List<Item> GetItemList()
    {
        return itemList;
    }

    /// <summary> 
    ///   Calculates the total number of items based on the amount of each item and returns it 
    /// </summary>
    public int GetTotalItemsCount()
    {
        int count = 0;
        foreach (Item item in itemList)
        {
            count += item.amount;
        }
        return count;
    }
}
