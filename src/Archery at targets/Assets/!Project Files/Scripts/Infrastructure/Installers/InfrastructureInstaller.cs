using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.UI;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.ProjectStateMachine;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Window;
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
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IInitializable>().To<IStaticDataService>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<ITargetFactory>().To<TargetFactory>().AsSingle();
        }

        private void BindProjectStateMachine()
        {
            Container.Bind<IProjectStateMachineService>().To<ProjectStateMachineService>().AsSingle();
        }
    }
}