#region

using Data.Paths;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.ARPlanes;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.XRSetup.TrackingMode;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

#endregion

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class PlaneAndBoundingBoxTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly IARPlanesService _arPlanesService;
        private readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        public PlaneAndBoundingBoxTrackingModeHandler(IARComponentsFactory arComponentsFactory,
            IARPlanesService arPlanesService, IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _arComponentsFactory = arComponentsFactory;
            _arPlanesService = arPlanesService;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARPlaneManager>();
            _arComponentsFactory.Remove<ARBoundingBoxManager>();

            _arPlanesService.SetArPlaneManager(null);
        }

        public async void Enable(IXRTrackingMode trackingMode)
        {
            var arPlaneManager = _arComponentsFactory.Create<ARPlaneManager>();
            var arBoundingBoxManager = _arComponentsFactory.Create<ARBoundingBoxManager>();

            _arPlanesService.SetArPlaneManager(arPlaneManager);

            trackingMode.ConfigureComponents(arPlaneManager, arBoundingBoxManager);

            arPlaneManager.planePrefab = await _assetsAddressablesProvider.GetAsset<GameObject>(
                AddressablesPaths.AR_PLANE_PREFAB);
            arBoundingBoxManager.boundingBoxPrefab = await _assetsAddressablesProvider.GetAsset<GameObject>(
                AddressablesPaths.AR_BOUNDED_BOX_PREFAB);
        }
    }
}