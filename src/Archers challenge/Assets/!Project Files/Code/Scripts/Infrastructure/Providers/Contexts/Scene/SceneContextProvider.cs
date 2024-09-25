#region

using Data.Contexts.Scene;

#endregion

namespace Infrastructure.Providers.SceneContainer
{
    public class SceneContextProvider : ISceneContextProvider
    {
        private BaseSceneContextData _sceneContextData;

        public void Set(BaseSceneContextData baseSceneContext) => _sceneContextData = baseSceneContext;

        public T Get<T>() where T : BaseSceneContextData => (T)_sceneContextData;
    }
}