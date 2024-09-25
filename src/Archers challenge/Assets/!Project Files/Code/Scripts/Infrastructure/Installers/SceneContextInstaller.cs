#region

using Data.Contexts.Scene;
using Infrastructure.Providers.SceneContainer;
using UnityEngine;
using Zenject;

#endregion

namespace Infrastructure.Installers
{
    public class SceneContextInstaller : MonoInstaller
    {
        [SerializeField] private BaseSceneContextData sceneContextData;

        public override void InstallBindings()
        {
            var sceneContainerProvider = Container.Resolve<ISceneContextProvider>();
            sceneContainerProvider.Set(sceneContextData);
        }
    }
}