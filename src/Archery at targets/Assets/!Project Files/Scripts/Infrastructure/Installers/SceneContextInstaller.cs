using Data.SceneContainer;
using Infrastructure.Services.SceneContainer;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class SceneContextInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuSceneContextData sceneContext;

        public override void InstallBindings()
        {
            var sceneContainerProvider = Container.Resolve<ISceneContextProvider>();
            sceneContainerProvider.Set(sceneContext);
        }
    }
}