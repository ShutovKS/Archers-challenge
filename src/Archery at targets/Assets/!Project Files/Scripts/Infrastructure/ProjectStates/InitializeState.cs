using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.ProjectStateMachine;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class InitializeState : IState, IEnterable
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IProjectStateMachine _projectStateMachine;
        private readonly IXRSetupService _xrSetupService;
        private readonly IStaticDataService _staticDataService;

        public InitializeState(
            IPlayerFactory playerFactory,
            IProjectStateMachine projectStateMachine,
            IXRSetupService xrSetupService,
            IStaticDataService staticDataService)
        {
            _playerFactory = playerFactory;
            _projectStateMachine = projectStateMachine;
            _xrSetupService = xrSetupService;
            _staticDataService = staticDataService;
        }

        public async void OnEnter()
        {
            await CreatePlayer();

            SetVrMode();

            MoveToNextState();
        }

        private async Task CreatePlayer()
        {
            await _playerFactory.CreatePlayer();
        }

        private void SetVrMode()
        {
            _xrSetupService.SetXRMode(XRMode.VR);
        }
        
        private void MoveToNextState()
        {
            _projectStateMachine.SwitchState<MainMenuState>();
        }
    }
}