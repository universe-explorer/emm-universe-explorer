using System;

/*
 * This level System should be encapsulated into game Object that implements Monobehaviour.
 * 
 * To enable the game object reacting to experience changes, subscribe to the OnExperienceChanged event.
 *      
 * To enable the game object reacting to level changes, subscribe to the OnLevelChanged event.
 *      
 */
public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    
    private int level;
    private int experience;
    private int experienceToNextLevel;

    private Inventory inventory;

    public LevelSystem()
    {
        level = 1;
        experience = 0;
        experienceToNextLevel = 100;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        while (experience >= experienceToNextLevel)
        {
            level++;
            experience -= experienceToNextLevel;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

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

    public int GetLevelNumber()
    {
        return level;
    }
}
