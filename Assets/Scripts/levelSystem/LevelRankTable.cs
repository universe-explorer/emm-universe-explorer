using System.Collections.Generic;

/// <summary> 
///   Provides Level Data lookup utilities
/// </summary>
public class LevelRankTable
{
    private static Dictionary<int, RankEntry> table = null;

    /// <summary> 
    ///   Represents level Rank for all levels 
    /// </summary>
    private static readonly List<RankEntry> rankEntries = new List<RankEntry>{
       new RankEntry { mineralRequired = 20, manaRequired = 20, medkitRequired = 1, healthRequired = 5},
       new RankEntry { mineralRequired = 30, manaRequired = 40, medkitRequired = 2, healthRequired = 10},
       new RankEntry { mineralRequired = 50, manaRequired = 60, medkitRequired = 3, healthRequired = 15},
       new RankEntry { mineralRequired = 60, manaRequired = 80, medkitRequired = 4, healthRequired = 20},
       new RankEntry { mineralRequired = 80, manaRequired = 100, medkitRequired = 5, healthRequired = 25},
    };

    /// <summary> 
    ///   Returns the Level Rank for all levels, starting from the Level 1
    /// </summary>
    public static Dictionary<int, RankEntry> GetLevelTable()
    {
        if (table == null)
        {
            table = new Dictionary<int, RankEntry>();
            for (int level = 0; level < rankEntries.Count; level++)
            {
                table.Add(level + 1, rankEntries[level]);
            }
        }
        return table;
    }

    /// <summary> 
    ///   Returns the Level Rank List for all levels
    /// </summary>
    public static List<RankEntry> GetLevelRankList()
    {
        return rankEntries;
    }
}
