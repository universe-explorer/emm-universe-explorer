using System.Collections.Generic;

/// <summary> 
///   Provides Level Data lookup utilities
/// </summary>
public class LevelRankTable
{
    private static Dictionary<int, PlayerRankEntry> playerRankTable = null;
    private static Dictionary<int, ItemRankEntry> itemRankTable = null;

    /// <summary> 
    ///   Represents Player level Rank for all levels 
    /// </summary>
    private static readonly List<PlayerRankEntry> playerRankEntries = new List<PlayerRankEntry>{
       new PlayerRankEntry { MaxVelocity = 30, BoostDuration = 180, DamageFactor = 1f},
       new PlayerRankEntry { MaxVelocity = 40, BoostDuration = 200, DamageFactor = 2f},
       new PlayerRankEntry { MaxVelocity = 50, BoostDuration = 220, DamageFactor = 3f},
       new PlayerRankEntry { MaxVelocity = 60, BoostDuration = 260, DamageFactor = 4f},
       new PlayerRankEntry { MaxVelocity = 70, BoostDuration = 320, DamageFactor = 5f},
    };

    /// <summary> 
    ///   Represents Item level Rank for all levels 
    /// </summary>
    private static readonly List<ItemRankEntry> itemRankEntries = new List<ItemRankEntry>{
       new ItemRankEntry { MineralRequired = 5, ManaRequired = 3, MedkitRequired = 1, HealthRequired = 3},
       new ItemRankEntry { MineralRequired = 20, ManaRequired = 20, MedkitRequired = 2, HealthRequired = 10},
       new ItemRankEntry { MineralRequired = 30, ManaRequired = 30, MedkitRequired = 3, HealthRequired = 15},
       new ItemRankEntry { MineralRequired = 40, ManaRequired = 40, MedkitRequired = 4, HealthRequired = 20},
       new ItemRankEntry { MineralRequired = 50, ManaRequired = 50, MedkitRequired = 5, HealthRequired = 25},
    };

    /// <summary> 
    ///   Returns the Player Level Rank for all levels, starting from the Level 1
    /// </summary>
    public static Dictionary<int, PlayerRankEntry> GetPlayerLevelTable()
    {
        if (playerRankTable == null)
        {
            playerRankTable = new Dictionary<int, PlayerRankEntry>();
            for (int level = 0; level < playerRankEntries.Count; level++)
            {
                playerRankTable.Add(level + 1, playerRankEntries[level]);
            }
        }
        return playerRankTable;
    }

    /// <summary> 
    ///   Returns the Item Level Rank for all levels, starting from the Level 1
    /// </summary>
    public static Dictionary<int, ItemRankEntry> GetItemLevelTable()
    {
        if (itemRankTable == null)
        {
            itemRankTable = new Dictionary<int, ItemRankEntry>();
            for (int level = 0; level < itemRankEntries.Count; level++)
            {
                itemRankTable.Add(level + 1, itemRankEntries[level]);
            }
        }
        return itemRankTable;
    }

    /// <summary> 
    ///   Returns the Item Level Rank List for all levels
    /// </summary>
    public static List<ItemRankEntry> GetItemLevelRankList()
    {
        return itemRankEntries;
    }
}
