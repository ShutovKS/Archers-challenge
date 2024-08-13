using Infrastructure.Services.ProjectStateMachine;
using Infrastructure.Services.SceneDependency;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IProjectStateMachineService>().To<ProjectStateMachineService>().AsSingle();
            Container.Bind<ISceneDependencyProvider>().To<SceneDependencyProvider>().AsSingle();
        }
    }
}