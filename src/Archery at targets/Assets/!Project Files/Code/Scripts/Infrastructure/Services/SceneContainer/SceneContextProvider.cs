#region

using Data.SceneContext;
using JetBrains.Annotations;

#endregion

namespace Infrastructure.Services.SceneContainer
{
    [UsedImplicitly]
    public class SceneContextProvider : ISceneContextProvider
    {
        private BaseSceneContextData _sceneContextData;

        public void Set(BaseSceneContextData baseSceneContext)
        {
            _sceneContextData = baseSceneContext;
        }

        public T Get<T>() where T : BaseSceneContextData
        {
            return (T)_sceneContextData;
        }
    }
}