using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.LevelGameplay;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.UI;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneContainer;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Zenject;
using ITickable = Zenject.ITickable;

namespace Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindFactories();
        }

        private void BindServices()
        {
            Container.Bind(typeof(IInitializable), typeof(IAssetsAddressablesProvider)).To<AssetsAddressablesProvider>().AsSingle();
            Container.Bind(typeof(IInitializable), typeof(IStaticDataService)).To<StaticDataService>().AsSingle();
            Container.Bind<IProjectManagementService>().To<ProjectStateMachine>().AsSingle();
            Container.Bind<IInteractorSetupService>().To<InteractorSetupService>().AsSingle();
            Container.Bind<ISceneContextProvider>().To<SceneContextProvider>().AsSingle();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
            Container.Bind(typeof(IStopwatchService), typeof(ITickable)).To<StopwatchService>().AsSingle();
            Container.Bind(typeof(ITimerService), typeof(ITickable)).To<TimerService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<IXRSetupService>().To<XRSetupService>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<IARComponentsFactory>().To<ARComponentsFactory>().AsSingle();
            Container.Bind<ITargetFactory>().To<TargetFactory>().AsSingle();
            Container.Bind<IGameplayLevelFactory>().To<GameplayLevelFactory>().AsSingle();
        }
    }
}