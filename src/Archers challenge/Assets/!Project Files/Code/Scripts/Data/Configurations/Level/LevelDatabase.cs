#region

using Data.Configurations.Database;
using UnityEngine;

#endregion

namespace Data.Configurations.Level
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Data/Level/Database", order = 0)]
    public class LevelDatabase : Database<LevelData>
    {
    }
}