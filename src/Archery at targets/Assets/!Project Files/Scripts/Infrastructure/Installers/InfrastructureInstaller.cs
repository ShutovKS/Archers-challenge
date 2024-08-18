using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.UI;
using Infrastructure.ProjectStateMachine;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindFactories();
            BindProjectStateMachine();
        }

        private void BindServices()
        {
            Container.Bind<IAssetsAddressablesProvider>().To<AssetsAddressablesProvider>().AsSingle();
            Container.Bind<IStopwatchService>().To<StopwatchService>().AsSingle();
            Container.Bind<ITimerService>().To<TimerService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();

            Container.Bind(typeof(StaticDataService), typeof(IStaticDataService), typeof(IInitializable))
                .To<StaticDataService>()
                .AsSingle();

            Container.Bind(typeof(XRSetupService), typeof(IXRSetupService), typeof(IInitializable))
                .To<XRSetupService>()
                .AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<ITargetFactory>().To<TargetFactory>().AsSingle();
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<IARComponentsFactory>().To<ARComponentsFactory>().AsSingle();
        }

        private void BindProjectStateMachine()
        {
            Container.Bind<IProjectStateMachine>().To<ProjectStateMachine.ProjectStateMachine>().AsSingle();
        }
    }
}