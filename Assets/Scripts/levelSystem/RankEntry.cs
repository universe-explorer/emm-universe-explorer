using System;
using System.Text;

/// <summary> 
///   Represents Items Properties required for individual level
///   Item Property: mineral, Mana, Medkit, Health
/// </summary>
public class RankEntry : IComparable<RankEntry>
{
    public int mineralRequired { get; set; }

    public int manaRequired { get; set; }

    public int medkitRequired { get; set; }

    public int healthRequired { get; set; }

    /// <summary> 
    ///   Compares RankEntry Objects
    /// </summary>
    public int CompareTo(RankEntry other)
    {
        if (this == other)
        {
            return 0;
        }
        if (mineralRequired >= other.mineralRequired
            && manaRequired >= other.manaRequired
            && medkitRequired >= other.medkitRequired
            && healthRequired >= other.healthRequired)
        {
            return 1;
        }
        return -1;
    }

    public override string ToString() {
        StringBuilder entry = new StringBuilder();

        entry.Append("RankEntry ");

        entry.Append(string.Concat("Mineral: ", mineralRequired.ToString())).Append(" ");
        entry.Append(string.Concat("Mana: ", manaRequired.ToString())).Append(" ");
        entry.Append(string.Concat("Medkit: ", medkitRequired.ToString())).Append(" ");
        entry.Append(string.Concat("Health: ", healthRequired.ToString())).Append(" ");

        return entry.ToString();
    }
}
