using System;
using System.Text;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        HealthPotion,
        ManaPotion,
        Mineral,
        Medkit,
    }

    public ItemType itemType;
    public int amount;

    public int healthPortion;
    public float maxSpeed;
    public int manaPortion;
    public int medkitPortion;

    /// <summary> 
    ///   Returns a summary of this Item with all the properties
    /// </summary>
    public string GetInfoText()
    {
        StringBuilder info = new StringBuilder();

        info.Append(GetTitle());
        info.Append(Environment.NewLine);

        info.Append(string.Concat("Health Portion: ", healthPortion));
        info.Append(Environment.NewLine);
        info.Append(string.Concat("Max Speed: ", maxSpeed.ToString()));
        info.Append(Environment.NewLine);
        info.Append(string.Concat("Mama Portion: ", manaPortion));
        info.Append(Environment.NewLine);
        info.Append(string.Concat("Medkit Portion: ", medkitPortion));

        return info.ToString();
    }

    /// <summary> 
    ///   Returns the associated Sprite of the individual Item Type
    /// </summary>
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.ManaPotion:   return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Mineral:      return ItemAssets.Instance.mineralSprite;
            case ItemType.Medkit:       return ItemAssets.Instance.medkitSprite;
        }
    }

    /// <summary> 
    ///   Checks whether this Item is stackable
    /// </summary>
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
            case ItemType.Mineral:
                return true;
            case ItemType.Medkit:
                return false;
        }
    }

    /// <summary> 
    ///   Returns Type of this Item
    /// </summary>
    private string GetTitle()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return "Health";
            case ItemType.ManaPotion:   return "Mana";
            case ItemType.Mineral:      return "Mineral";
            case ItemType.Medkit:       return "Medkit";
        }
    }
}
