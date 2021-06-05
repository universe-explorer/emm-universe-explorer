using System;
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

    // TODO add more custom properties

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.ManaPotion: return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Mineral: return ItemAssets.Instance.mineralSprite;
            case ItemType.Medkit: return ItemAssets.Instance.medkitSprite;
        }
    }
}
