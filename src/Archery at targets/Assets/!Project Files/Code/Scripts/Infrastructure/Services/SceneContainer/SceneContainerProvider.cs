#region

using Data.SceneContext;

#endregion

namespace Infrastructure.Services.SceneContainer
{
    public interface ISceneContextProvider
    {
        void Set(BaseSceneContextData baseSceneContext);
        T Get<T>() where T : BaseSceneContextData;
    }
}