#region

using Data.SceneContext;
using Infrastructure.Services.SceneContainer;
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