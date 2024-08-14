using Infrastructure.ProjectStates;
using Infrastructure.Services.ProjectStateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure
{
    [UsedImplicitly]
    public class GamingBootloader : IInitializable
    {
        private readonly IProjectStateMachineService _projectStateMachineService;

        public GamingBootloader(IProjectStateMachineService projectStateMachineService)
        {
            _projectStateMachineService = projectStateMachineService;
        }

        public void Initialize()
        {
            _projectStateMachineService.SwitchState<BootstrapState>();
        }
    }
}