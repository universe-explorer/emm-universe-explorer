using System.Collections.Generic;

/// <summary> 
///   Provides Level Data lookup utilities
/// </summary>
public class LevelRankTable
{
    private static Dictionary<int, PlayerRankEntry> playerRankTable = null;
    private static Dictionary<int, ItemRankEntry> itemRankTable = null;

    /// <summary> 
    ///   Represents player level Rank for all levels 
    /// </summary>
    private static readonly List<PlayerRankEntry> playerRankEntries = new List<PlayerRankEntry>{
       new PlayerRankEntry { MaxVelocity = 30, BoostDuration = 120 },
       new PlayerRankEntry { MaxVelocity = 40, BoostDuration = 140 },
       new PlayerRankEntry { MaxVelocity = 50, BoostDuration = 160 },
       new PlayerRankEntry { MaxVelocity = 60, BoostDuration = 180 },
       new PlayerRankEntry { MaxVelocity = 70, BoostDuration = 200 },
    };

    /// <summary> 
    ///   Represents item level Rank for all levels 
    /// </summary>
    private static readonly List<ItemRankEntry> itemRankEntries = new List<ItemRankEntry>{
       new ItemRankEntry { MineralRequired = 20, ManaRequired = 20, MedkitRequired = 1, HealthRequired = 5},
       new ItemRankEntry { MineralRequired = 30, ManaRequired = 40, MedkitRequired = 2, HealthRequired = 10},
       new ItemRankEntry { MineralRequired = 50, ManaRequired = 60, MedkitRequired = 3, HealthRequired = 15},
       new ItemRankEntry { MineralRequired = 60, ManaRequired = 80, MedkitRequired = 4, HealthRequired = 20},
       new ItemRankEntry { MineralRequired = 80, ManaRequired = 100, MedkitRequired = 5, HealthRequired = 25},
    };

    /// <summary> 
    ///   Returns the player Level Rank for all levels, starting from the Level 1
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
    ///   Returns the item Level Rank for all levels, starting from the Level 1
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
    ///   Returns the Level Rank List for all levels
    /// </summary>
    public static List<ItemRankEntry> GetLevelRankList()
    {
        return itemRankEntries;
    }
}
