using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.Weapon;

namespace Core.Project.Gameplay
{
    public class WaitingToStartState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;
        private readonly IWeaponService _weaponService;

        public WaitingToStartState(
            IProjectManagementService projectManagementService,
            IWeaponService weaponService
        )
        {
            _projectManagementService = projectManagementService;
            _weaponService = weaponService;
        }

        public void OnEnter()
        {
            _weaponService.CurrentWeapon.OnSelected += WeaponSelected;
        }

        private void WeaponSelected(bool isSelected)
        {
            if (isSelected)
            {
                _weaponService.CurrentWeapon.OnSelected -= WeaponSelected;

                MoveToNextState();
            }
        }

        private void MoveToNextState() => _projectManagementService.ChangeState<GameProcessState>();
    }
}