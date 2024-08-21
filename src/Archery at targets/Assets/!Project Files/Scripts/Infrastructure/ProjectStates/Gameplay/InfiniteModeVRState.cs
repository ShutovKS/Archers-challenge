// using System.Threading.Tasks;
// using Data.Level;
// using Extension;
// using Infrastructure.Factories.GameObjects;
// using Infrastructure.Factories.Player;
// using Infrastructure.ProjectStateMachine;
// using Infrastructure.Services.InteractorSetup;
// using Infrastructure.Services.StaticData;
// using Infrastructure.Services.XRSetup;
// using Infrastructure.Services.XRSetup.TrackingMode;
// using UnityEngine;
//
// namespace Infrastructure.ProjectStates.Gameplay
// {
//     public class InfiniteModeVRState : IState, IEnterable, IExitable
//     {
//         private readonly IXRSetupService _xrSetupService;
//         private readonly IInteractorSetupService _interactorSetupService;
//         private readonly IGameObjectFactory _gameObjectFactory;
//         private readonly IPlayerFactory _playerFactory;
//         private readonly IStaticDataService _staticDataService;
//
//         private GameObject _locationInstance;
//         private InfiniteVRLevelData _levelData;
//
//         public InfiniteModeVRState(IXRSetupService xrSetupService,
//             IInteractorSetupService interactorSetupService,
//             IGameObjectFactory gameObjectFactory,
//             IPlayerFactory playerFactory,
//             IStaticDataService staticDataService)
//         {
//             _xrSetupService = xrSetupService;
//             _interactorSetupService = interactorSetupService;
//             _gameObjectFactory = gameObjectFactory;
//             _playerFactory = playerFactory;
//             _staticDataService = staticDataService;
//         }
//
//         public async void OnEnter()
//         {
//             InitializeData();
//
//             await InstantiateLocation();
//
//             ConfigurePlayer();
//         }
//
//         private void InitializeData()
//         {
//             _levelData = _staticDataService.GetLevelData<InfiniteVRLevelData>("InfiniteVRLevelData");
//         }
//
//         private async Task InstantiateLocation()
//         {
//             var sceneReference = _levelData.LocationSceneReference;
//             _locationInstance = await _gameObjectFactory.Instance(sceneReference);
//         }
//
//         private void ConfigurePlayer()
//         {
//             _xrSetupService.SetXRMode(XRMode.VR);
//             _xrSetupService.SetXRTrackingMode(new NoneTrackingMode());
//             _xrSetupService.SetAnchorManagerState(false);
//
//             _interactorSetupService.SetInteractor(InteractorType.NearFar);
//
//             _playerFactory.Player.SetPositionAndRotation(_levelData.PlayerSpawnPoint);
//         }
//
//         public void OnExit()
//         {
//             Object.Destroy(_locationInstance);
//         }
//     }
// }