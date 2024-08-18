using Data.Level;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        void Initialize();
        TLevelData GetLevelData<TLevelData>(string key) where TLevelData : BaseLevelData;
    }
}