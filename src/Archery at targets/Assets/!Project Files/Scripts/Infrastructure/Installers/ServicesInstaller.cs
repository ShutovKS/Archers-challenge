using Infrastructure.Services.ProjectStateMachine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IProjectStateMachineService>().To<ProjectStateMachineService>().AsSingle();
        }
    }
}