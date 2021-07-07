using System;
using System.Text;

/// <summary> 
///   Represents Items Properties required for individual level
///   Item Property: mineral, Mana, Medkit, Health
/// </summary>
public class ItemRankEntry : IComparable<ItemRankEntry>
{
    public int HealthRequired { get; set; }

    public int ManaRequired { get; set; }

    public int MineralRequired { get; set; }

    public int MedkitRequired { get; set; }

    /// <summary>
    ///   Compares RankEntry Objects
    /// </summary>
    /// <param name="other">Other ItemRankEntry to be compared with</param>
    /// <returns></returns>
    public int CompareTo(ItemRankEntry other)
    {
        if (this == other)
        {
            return 0;
        }
        if (MineralRequired >= other.MineralRequired
            && ManaRequired >= other.ManaRequired
            && MedkitRequired >= other.MedkitRequired
            && HealthRequired >= other.HealthRequired)
        {
            return 1;
        }
        return -1;
    }

    /// <summary>
    ///   String representation of a ItemRankEntry
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        StringBuilder entry = new StringBuilder();

        entry.Append("ItemRankEntry ");

        entry.Append(string.Concat("Health: ", HealthRequired.ToString())).Append(" ");
        entry.Append(string.Concat("Mana: ", ManaRequired.ToString())).Append(" ");
        entry.Append(string.Concat("Mineral: ", MineralRequired.ToString())).Append(" ");
        entry.Append(string.Concat("Medkit: ", MedkitRequired.ToString())).Append(" ");

        return entry.ToString();
    }
}
