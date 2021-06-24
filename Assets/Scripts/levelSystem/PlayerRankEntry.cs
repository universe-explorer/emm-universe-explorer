using System.Text;

/// <summary> 
///   Represents Player's Properties for individual level
///   Player's Properties: MaxVelocity, BoostDuration
/// </summary>
public class PlayerRankEntry
{
    public float MaxVelocity { get; set; }

    public int BoostDuration { get; set; }

    public override string ToString()
    {
        StringBuilder entry = new StringBuilder();

        entry.Append("PlayerRankEntry ");

        entry.Append(string.Concat("MaxVelocity: ", MaxVelocity.ToString())).Append(" ");
        entry.Append(string.Concat("BoostDuration: ", BoostDuration.ToString())).Append(" ");

        return entry.ToString();
    }
}
