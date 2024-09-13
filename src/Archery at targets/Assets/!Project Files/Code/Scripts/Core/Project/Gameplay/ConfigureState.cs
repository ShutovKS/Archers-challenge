using Data.Configurations.Level;
using Data.Contexts.Scene;
using Infrastructure.Providers.GlobalDataContainer;
using Infrastructure.Providers.SceneContainer;
using Infrastructure.Services.InteractorSetup;
using Infrastructure.Services.Player;
using Infrastructure.Services.ProjectManagement;
using Infrastructure.Services.XRSetup;
using UnityEngine;

namespace Core.Project.Gameplay
{
    public class ConfigureState : IState, IEnterable
    {
        private readonly IGlobalContextProvider _globalContextProvider;
        private readonly ISceneContextProvider _sceneContextProvider;
        private readonly IXRSetupService _xrSetupService;
        private readonly IInteractorService _interactorService;
        private readonly IPlayerService _playerService;
        private readonly IProjectManagementService _projectManagementService;

        public ConfigureState(
            IGlobalContextProvider globalContextProvider,
            IProjectManagementService projectManagementService,
            ISceneContextProvider sceneContextProvider,
            IXRSetupService xrSetupService,
            IInteractorService interactorService,
            IPlayerService playerService
        )
        {
            _globalContextProvider = globalContextProvider;
            _sceneContextProvider = sceneContextProvider;
            _xrSetupService = xrSetupService;
            _interactorService = interactorService;
            _playerService = playerService;
            _projectManagementService = projectManagementService;
        }

        public void OnEnter()
        {
            var levelData = _globalContextProvider.GlobalContext.LevelData;
            var sceneContextData = _sceneContextProvider.Get<GameplaySceneContextData>();

            ConfigurePlayer(levelData, sceneContextData.PlayerSpawnPoint);

            MoveToNextState();
        }

        private void ConfigurePlayer(LevelData levelData, Transform playerSpawnPoint)
        {
            _xrSetupService.SetXRMode(levelData.XRMode);

            _interactorService.SetUpInteractor(HandType.Left, InteractorType.NearFar);
            _interactorService.SetUpInteractor(HandType.Right, InteractorType.Direct | InteractorType.Poke);

            _playerService.SetPlayerPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        }

        private void MoveToNextState() => _projectManagementService.ChangeState<WaitingToStartState>();
    }
}