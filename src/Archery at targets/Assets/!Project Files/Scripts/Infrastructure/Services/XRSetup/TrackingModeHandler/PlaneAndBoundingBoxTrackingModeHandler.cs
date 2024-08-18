using System.Threading.Tasks;
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

        public async Task Enable(IXRTrackingMode trackingMode)
        {
            var arPlaneManager = await _arComponentsFactory.Create<ARPlaneManager>();
            var arBoundingBoxManager = await _arComponentsFactory.Create<ARBoundingBoxManager>();
            trackingMode.ConfigureComponents(arPlaneManager, arBoundingBoxManager);
        }
    }
}