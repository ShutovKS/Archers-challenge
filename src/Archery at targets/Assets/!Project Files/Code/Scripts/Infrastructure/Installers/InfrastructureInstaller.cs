#region

using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.ARTrackingMode;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.GameplayLevels;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Projectile;
using Infrastructure.Factories.ProjectStates;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.UI;
using Infrastructure.Observers.ProgressData;
using Infrastructure.Providers.AssetsAddressables;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Providers.Interactor;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Providers.StaticData;
using Infrastructure.Services.ARPlanes;
using Infrastructure.Services.Camera;
using Infrastructure.Services.DataStorage;
using Infrastructure.Services.GameSetup;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Projectile;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Weapon;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Zenject;
using ITickable = Zenject.ITickable;

#endregion

namespace Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindProviders();
            BindFactories();
            BindObservers();
            BindServices();
        }

        private void BindServices()
        {
            Container.Bind<IARPlanesService>().To<ARPlanesService>().AsSingle();
            Container.Bind<ICameraService>().To<CameraService>().AsSingle();
            Container.Bind<IDataStorageService>().To<DataStorageLocalService>().AsSingle();
            Container.Bind(typeof(IInteractorService), typeof(IInteractorProvider)).To<InteractorService>().AsSingle();
            Container.Bind<IGameplaySetupService>().To<GameplaySetupService>().AsSingle();
            Container.Bind<IMainMenuSetupService>().To<MainMenuSetupService>().AsSingle();
            Container.Bind<IPlayerService>().To<PlayerService>().AsSingle();
            Container.Bind<IProgressService>().To<ProgressService>().AsSingle();
            Container.Bind<IProjectileService>().To<ProjectileService>().AsSingle();
            Container.Bind<IProjectManagementService>().To<ProjectStateMachine>().AsSingle();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
            Container.Bind(typeof(IStopwatchService), typeof(ITickable)).To<StopwatchService>().AsSingle();
            Container.Bind(typeof(ITimerService), typeof(ITickable)).To<TimerService>().AsSingle();
            Container.Bind(typeof(IInitializable), typeof(IWeaponService)).To<WeaponService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<IXRSetupService>().To<XRSetupService>().AsSingle();
        }
        
        private void BindProviders()
        {
            Container.Bind(typeof(IInitializable), typeof(IAssetsAddressablesProvider)).To<AssetsAddressablesProvider>().AsSingle();
            Container.Bind<IGlobalContextProvider>().To<GlobalContextProvider>().AsSingle();
            Container.Bind<ISceneContextProvider>().To<SceneContextProvider>().AsSingle();
            Container.Bind(typeof(IInitializable), typeof(IStaticDataProvider)).To<StaticDataProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IARComponentsFactory>().To<ARComponentsFactory>().AsSingle();
            Container.Bind<IARTrackingModeFactory>().To<ARTrackingModeFactory>().AsSingle();
            Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
            Container.Bind<IGameplayLevelsFactory>().To<GameplayLevelsFactory>().AsSingle();
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();
            Container.Bind<IProjectStatesFactory>().To<ProjectStatesFactory>().AsSingle();
            Container.Bind<ITargetFactory>().To<TargetFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }

        private void BindObservers()
        {
            Container.Bind<IProgressDataObserver>().To<ProgressDataObserver>().AsSingle();
        }
    }
}