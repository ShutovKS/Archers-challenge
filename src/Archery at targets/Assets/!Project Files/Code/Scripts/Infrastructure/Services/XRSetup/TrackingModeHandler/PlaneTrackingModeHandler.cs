using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.ARPlanes;
using Infrastructure.Services.XRSetup.TrackingMode;
using JetBrains.Annotations;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    [UsedImplicitly]
    public class PlaneTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly IARPlanesService _arPlanesService;

        public PlaneTrackingModeHandler(IARComponentsFactory arComponentsFactory, IARPlanesService arPlanesService)
        {
            _arComponentsFactory = arComponentsFactory;
            _arPlanesService = arPlanesService;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARPlaneManager>();

            _arPlanesService.SetArPlaneManager(null);
        }

        public void Enable(IXRTrackingMode trackingMode)
        {
            var arPlaneManager = _arComponentsFactory.Create<ARPlaneManager>();

            _arPlanesService.SetArPlaneManager(arPlaneManager);

            trackingMode.ConfigureComponents(arPlaneManager);
        }
    }
}