#region

using Infrastructure.Services.ProjectManagement;

#endregion

namespace Core.Project.Gameplay
{
    public class GameplayState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public GameplayState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void OnEnter() => MoveToNextState();

        private void MoveToNextState() => _projectManagementService.ChangeState<LocationBootState>();
    }
}