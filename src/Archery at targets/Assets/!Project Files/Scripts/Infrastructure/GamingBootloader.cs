using Infrastructure.GameStates;
using Infrastructure.Services.ProjectStateMachine;
using Zenject;

namespace Infrastructure
{
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