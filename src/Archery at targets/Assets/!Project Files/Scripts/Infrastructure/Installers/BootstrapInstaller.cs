using Infrastructure.ProjectStateMachine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameBootstrap>().AsSingle().NonLazy();
        }

        public override void Start()
        {
            base.Start();

            Container.Resolve<GameBootstrap>().Initialize();
        }
    }
}