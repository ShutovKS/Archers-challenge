using Data.SceneContext;
using Infrastructure.Services.SceneContainer;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class SceneContextInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuSceneContextData mainMenuSceneContextData;

        public override void InstallBindings()
        {
            var sceneContainerProvider = Container.Resolve<ISceneContextProvider>();
            sceneContainerProvider.Set(mainMenuSceneContextData);
        }
    }
}