using System.Text;

/// <summary> 
///   Represents Player's Properties for individual level
///   Player's Properties: MaxVelocity, BoostDuration
/// </summary>
public class PlayerRankEntry
{
    /// <summary>
    ///   Max Velocity the Player could have
    /// </summary>
    public float MaxVelocity { get; set; }

    /// <summary>
    ///   Boost Duration that will get updated while upgrading or downgrading
    /// </summary>
    public int BoostDuration { get; set; }


    /// <summary>
    ///   Damage Factor the Player could have
    /// </summary>
    public float DamageFactor { get; set; }

    /// <summary>
    ///   String representation of a PlayerRankEntry
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        StringBuilder entry = new StringBuilder();

        entry.Append("PlayerRankEntry ");

        entry.Append(string.Concat("MaxVelocity: ", MaxVelocity.ToString())).Append(" ");
        entry.Append(string.Concat("BoostDuration: ", BoostDuration.ToString())).Append(" ");
        entry.Append(string.Concat("DamageFactor: ", DamageFactor.ToString())).Append(" ");

        return entry.ToString();
    }
}
