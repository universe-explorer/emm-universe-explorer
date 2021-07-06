using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Health,
        Mana,
        Mineral,
        Medkit,
    }

    public ItemType itemType;
    public int amount;

    /// <summary> 
    ///   Returns the associated Sprite of the individual Item Type
    /// </summary>
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Health:       return ItemAssets.Instance.healthPotionSprite;
            case ItemType.Mana:         return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Mineral:      return ItemAssets.Instance.mineralSprite;
            case ItemType.Medkit:       return ItemAssets.Instance.medkitSprite;
        }
    }

    /// <summary> 
    ///   Returns Type of this Item
    /// </summary>
    public string GetTitle()
    {
        switch (itemType)
        {
            default:
            case ItemType.Health:       return "Health Item";
            case ItemType.Mana:         return "Mana Item";
            case ItemType.Mineral:      return "Mineral Item";
            case ItemType.Medkit:       return "Medkit Item";
        }
    }
}
