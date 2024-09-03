#region

using Data.Database;
using UnityEngine;

#endregion

namespace Data.Level
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Data/Level/Database", order = 0)]
    public class LevelDatabase : Database<LevelData> { }
}