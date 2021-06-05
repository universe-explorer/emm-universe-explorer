using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        Debug.Log("Inventory item counter: " + itemList.Count);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
