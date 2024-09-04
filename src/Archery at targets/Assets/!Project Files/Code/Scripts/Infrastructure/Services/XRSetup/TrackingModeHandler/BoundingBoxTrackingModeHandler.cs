#region

using Data.Path;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

#endregion

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class BoundingBoxTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        public BoundingBoxTrackingModeHandler(IARComponentsFactory arComponentsFactory,
            IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _arComponentsFactory = arComponentsFactory;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARBoundingBoxManager>();
        }

        public async void Enable(IXRTrackingMode trackingMode)
        {
            var arBoundingBoxManager = _arComponentsFactory.Create<ARBoundingBoxManager>();
            trackingMode.ConfigureComponents(arBoundingBoxManager);

            arBoundingBoxManager.boundingBoxPrefab = await _assetsAddressablesProvider.GetAsset<GameObject>(
                AddressablesPaths.AR_BOUNDED_BOX_PREFAB);
        }
    }
}