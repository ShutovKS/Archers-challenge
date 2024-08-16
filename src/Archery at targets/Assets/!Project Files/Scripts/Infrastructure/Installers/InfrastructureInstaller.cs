using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Target;
using Infrastructure.Factories.UI;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.ProjectStateMachine;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Stopwatch;
using Infrastructure.Services.Timer;
using Infrastructure.Services.Window;
using Infrastructure.Services.XRSetup;
using Infrastructure.Services.XRSetup.AR.Features;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindFactories();
            BindXRSetup();
            BindProjectStateMachine();
        }

        private void BindServices()
        {
            Container.Bind<IAssetsAddressablesProvider>().To<AssetsAddressablesProvider>().AsSingle();
            Container.Bind<IStopwatchService>().To<StopwatchService>().AsSingle();
            Container.Bind<ITimerService>().To<TimerService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind(typeof(StaticDataService), typeof(IStaticDataService), typeof(IInitializable))
                .To<StaticDataService>().AsSingle();
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

        private void BindXRSetup()
        {
            Container.Bind<IXRSetupService>().To<XRSetupService>().AsSingle();

            Container.BindInterfacesTo<ARSessionSetup>().AsSingle();
            Container.BindInterfacesTo<ARCameraSetup>().AsSingle();
            Container.BindInterfacesTo<ARPlaneSetup>().AsSingle();
            Container.BindInterfacesTo<ARBoundingBoxesSetup>().AsSingle();
            Container.BindInterfacesTo<ARAnchorSetup>().AsSingle();
            // Container.BindInterfacesTo<ARMeshSetup>().AsSingle();
        }
    }
}