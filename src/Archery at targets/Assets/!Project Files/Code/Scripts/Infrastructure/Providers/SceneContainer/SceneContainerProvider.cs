#region

using Data.Contexts.Scene;

#endregion

namespace Infrastructure.Providers.SceneContainer
{
    public interface ISceneContextProvider
    {
        void Set(BaseSceneContextData baseSceneContext);
        T Get<T>() where T : BaseSceneContextData;
    }
}