using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class PlaneAndBoundingBoxTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;

        public PlaneAndBoundingBoxTrackingModeHandler(IARComponentsFactory arComponentsFactory)
        {
            _arComponentsFactory = arComponentsFactory;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARPlaneManager>();
            _arComponentsFactory.Remove<ARBoundingBoxManager>();
        }

        public void Enable(IXRTrackingMode trackingMode)
        {
            var arPlaneManager = _arComponentsFactory.Create<ARPlaneManager>();
            var arBoundingBoxManager = _arComponentsFactory.Create<ARBoundingBoxManager>();
            trackingMode.ConfigureComponents(arPlaneManager, arBoundingBoxManager);
        }
    }
}