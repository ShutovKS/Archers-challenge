using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.ProjectStateMachine;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.XRSetup;
using JetBrains.Annotations;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class InitializeState : IState, IEnterable
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IProjectStateMachine _projectStateMachine;
        private readonly IXRSetupService _xrSetupService;
        private readonly IARComponentsFactory _arComponentsFactory;

        public InitializeState(
            IPlayerFactory playerFactory,
            IGameObjectFactory gameObjectFactory,
            IProjectStateMachine projectStateMachine,
            IXRSetupService xrSetupService,
            IARComponentsFactory arComponentsFactory)
        {
            _playerFactory = playerFactory;
            _gameObjectFactory = gameObjectFactory;
            _projectStateMachine = projectStateMachine;
            _xrSetupService = xrSetupService;
            _arComponentsFactory = arComponentsFactory;
        }

        public async void OnEnter()
        {
            await CreateARSession();
            await CreatePlayer();

            MoveToNextState();
        }

        private async Task CreatePlayer()
        {
            await _playerFactory.CreatePlayer();
            await _arComponentsFactory.CreateARComponent<ARCameraManager>();
        }

        private async Task CreateARSession()
        {
            await _arComponentsFactory.CreateARComponent<ARSession>();
        }

        private void MoveToNextState()
        {
            _projectStateMachine.SwitchState<MainMenuState>();
        }
    }
}