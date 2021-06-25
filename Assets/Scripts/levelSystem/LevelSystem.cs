using System;
using System.Collections.Generic;

/// <summary> 
///   This level System should be encapsulated into game Object that implements Monobehaviour.
///   To enable the game object reacting to experience changes, subscribe to the OnExperienceChanged event.
///   To enable the game object reacting to level changes, subscribe to the OnLevelChanged event.
/// </summary>
public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    
    private int currentLevel;

    private Inventory inventory;
    private List<ItemRankEntry> rankEntries;

    public LevelSystem()
    {
        currentLevel = 1;
        rankEntries = LevelRankTable.GetLevelRankList();
    }

    /// <summary> 
    ///   Sets Inventory which acts as a Item Repository for this level system
    /// </summary>
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemAdded += Inventory_OnItemAdded;
        inventory.OnItemRemoved += Inventory_OnItemRemoved;
    }

    private void Inventory_OnItemAdded(object sender, EventArgs e)
    {
        // Level Window UI reacts to the Item Added Events which in turn also update Bar's Value
        // we should first trigger ExperienceChanged Events and update level
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        Upgrade(GetChangedLevelRank());
    }

    private void Inventory_OnItemRemoved(object sender, EventArgs e)
    {
        // Level Window UI reacts to the Item Removed Events which in turn also update Bar's Value
        // we should first trigger ExperienceChanged Events and update level
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        Downgrade(GetChangedLevelRank());
    }

    /// <summary> 
    ///   Returns Item Properties associated with the current level
    /// </summary>
    public ItemRankEntry GetCurrentItemLevelRank()
    {
        return LevelRankTable.GetItemLevelTable()[currentLevel];
    }

    /// <summary> 
    ///   Returns Player Properties associated with the current level
    /// </summary>
    public PlayerRankEntry GetCurrentPlayerLevelRank()
    {
        return LevelRankTable.GetPlayerLevelTable()[currentLevel];
    }

    /// <summary> 
    ///   Returns a RankEntry which represents the incoming changed Item properties
    /// </summary>
    private ItemRankEntry GetChangedLevelRank()
    {
        return new ItemRankEntry
        {
            MineralRequired = GetMineralLevelValue(),
            ManaRequired = GetManaLevelValue(),
            MedkitRequired = GetMedkitLevelValue(),
            HealthRequired = GetHealthLevelValue(),
        };
    }

    /// <summary> 
    ///   Upgrades level according to the item's properties
    /// </summary>
    private void Upgrade(ItemRankEntry changedRank)
    {
        bool levelChanged = false;
        for (int levelIter = 0; levelIter < rankEntries.Count; levelIter++)
        {
            ItemRankEntry levelRank = rankEntries[levelIter];
            int level = levelIter + 1;
            if (changedRank.CompareTo(levelRank) >= 0 && level > currentLevel - 1)
            {
                currentLevel += 1;
                levelChanged = true;
            }
        }
        if (levelChanged && OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
    }

    /// <summary> 
    ///   Downgrades level according to the item's properties
    /// </summary>
    private void Downgrade(ItemRankEntry changedRank)
    {
        bool levelChanged = false;

        for (int levelIter = rankEntries.Count - 1; levelIter > 0; levelIter--)
        {
            ItemRankEntry levelRank = rankEntries[levelIter];
            if (changedRank.CompareTo(levelRank) < 0 && currentLevel > levelIter) 
            {
                currentLevel -= 1;
                levelChanged = true;
            }
        }
        if (levelChanged && OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
    }

    /**
     * TODO: cache returned value 
     * 
     * every time when space ship collects new items, the following Getters are called which 
     * cause a interation of the item list that runs in linear time, we should cache the
     * corresponding values, but in our usecase this should not be a performance issue because 
     * the item list is relative small.
     */

    /// <summary> 
    ///   Returns the total amount of the mineral Items within Inventory 
    /// </summary>
    public int GetMineralLevelValue()
    {
        int result = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (item.itemType == Item.ItemType.Mineral)
            {
                result += item.amount;
            }
        }
        return result;
    }

    /// <summary> 
    ///   Returns the total amount of the medkit Items within Inventory
    /// </summary>
    public int GetMedkitLevelValue()
    {
        int result = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (item.itemType == Item.ItemType.Medkit)
            {
                result += item.amount;
            }
        }
        return result;
    }

    /// <summary> 
    ///   Returns the total amount of the mana Items within Inventory
    /// </summary>
    public int GetManaLevelValue()
    {
        int result = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (item.itemType == Item.ItemType.ManaPotion)
            {
                result += item.amount;
            }
        }
        return result;
    }

    /// <summary> 
    ///   Returns the total amount of the health Items within Inventory
    /// </summary>
    public int GetHealthLevelValue()
    {
        int result = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (item.itemType == Item.ItemType.HealthPotion)
            {
                result += item.amount;
            }
        }
        return result;
    }

    /// <summary> 
    ///   Returns the current level number
    /// </summary>
    public int GetLevelNumber()
    {
        return currentLevel;
    }
}
