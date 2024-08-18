using Data.Level;
using Unity.VisualScripting;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        TLevelData GetLevelData<TLevelData>(string key) where TLevelData : BaseLevelData;
    }
}