﻿using System.Collections;
using UnityEngine;

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

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.ManaPotion: return ItemAssets.Instance.ManaPotionSprite;
            case ItemType.Mineral: return ItemAssets.Instance.MineralSprite;
            case ItemType.Medkit: return ItemAssets.Instance.medkitSprite;
        }
    }
}