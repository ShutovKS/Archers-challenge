#region

using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine.XR.ARFoundation;

#endregion

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class PlaneTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;

        public PlaneTrackingModeHandler(IARComponentsFactory arComponentsFactory)
        {
            _arComponentsFactory = arComponentsFactory;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARPlaneManager>();
        }

        public void Enable(IXRTrackingMode trackingMode)
        {
            var arPlaneManager = _arComponentsFactory.Create<ARPlaneManager>();
            trackingMode.ConfigureComponents(arPlaneManager);
        }
    }
}