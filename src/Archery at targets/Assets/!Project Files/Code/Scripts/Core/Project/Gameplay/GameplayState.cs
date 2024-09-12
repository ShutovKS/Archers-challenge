#region

using Core.Gameplay;
using Data.Configurations.Level;
using Features.PositionsContainer;
using Features.Weapon;
using Infrastructure.Services.ProjectManagement;
using UI.InformationDesk;

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

        public void OnEnter()
        {
            MoveToBootState();
        }

        private void MoveToBootState() => _projectManagementService.ChangeState<LocationBootState>();
    }
}