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
    
    private int currentlevel;

    private Inventory inventory;

    public LevelSystem()
    {
        currentlevel = 1;
    }

    /// <summary> 
    ///   Set Inventory which acts as a Item Repository for this level system
    /// </summary>
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    /// <summary> 
    ///   Returns Item Properties associated with the current level
    /// </summary>
    public RankEntry GetCurrentLevelRank()
    {
        return LevelRankTable.GetLevelTable()[currentlevel];
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        // Level Window UI reacts to the Item List Changed Events which in turn also update Bar's Value
        // we should first trigger ExperienceChanged Events and update level
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        UpdateLevel();
    }

    /// <summary> 
    ///   Updates level according the incoming item properties, which means either upgrading or downgrading
    /// </summary>
    private void UpdateLevel()
    {
        RankEntry changedRank = new RankEntry
        {
            mineralRequired = GetMineralLevelValue(),
            manaRequired = GetManaLevelValue(),
            medkitRequired = GetMedkitLevelValue(),
            healthRequired = GetHealthLevelValue(),
        };
        bool levelChanged = false;
        List<RankEntry> rankEntries = LevelRankTable.GetLevelRankList();
        for (int levelIter = 0; levelIter < rankEntries.Count; levelIter++)
        {
            RankEntry levelRank = rankEntries[levelIter];
            int level = levelIter + 1;
            if (changedRank.CompareTo(levelRank) < 0)
            {
                if (level < currentlevel - 1)
                {
                    currentlevel -= 1;
                    levelChanged = true;
                    /**
                     * iterate through the rank list in the defined order which also means 
                     * the following Rank inside the list does have higher item properties
                     * so we break the loop here
                     */
                    break;
                }
            }
            if (changedRank.CompareTo(levelRank) >= 0)
            {
                if (level > currentlevel - 1)
                {
                    currentlevel += 1;
                    levelChanged = true;
                    // we don not break th loop until we find the correct level to match up
                }
            }
        }
        if (levelChanged && OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
    }

    private int GetMaximumLevel()
    {
        return LevelRankTable.GetLevelRankList().Count;
    }

    /**
     * TODO: cache returned value 
     * 
     * every time when space ship collects new items, the following Getter are called which 
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
        return currentlevel;
    }
}
