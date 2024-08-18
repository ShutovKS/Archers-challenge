using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.GameObjects;
using Infrastructure.Factories.Player;
using Infrastructure.ProjectStateMachine;
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

        public InitializeState(
            IPlayerFactory playerFactory,
            IGameObjectFactory gameObjectFactory,
            IProjectStateMachine projectStateMachine,
            IXRSetupService xrSetupService)
        {
            _playerFactory = playerFactory;
            _gameObjectFactory = gameObjectFactory;
            _projectStateMachine = projectStateMachine;
            _xrSetupService = xrSetupService;
        }

        public async void OnEnter()
        {
            await CreatePlayer();

            MoveToNextState();
        }

        private async Task CreatePlayer()
        {
            await _playerFactory.CreatePlayer();
        }

        private void MoveToNextState()
        {
            _projectStateMachine.SwitchState<MainMenuState>();
        }
    }
}