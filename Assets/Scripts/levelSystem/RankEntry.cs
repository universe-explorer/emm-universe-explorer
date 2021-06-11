using System;

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

    // TODO
    public int CompareTo(RankEntry other)
    {
        throw new NotImplementedException();
    }
}
