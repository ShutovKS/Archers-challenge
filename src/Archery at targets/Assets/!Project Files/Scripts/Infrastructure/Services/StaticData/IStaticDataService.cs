using Data.Level;
using Zenject;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        new void Initialize();
        TLevelData GetLevelData<TLevelData>(string key) where TLevelData : BaseLevelData;
    }
}