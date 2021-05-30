using System;
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
}
