using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine.XR.ARFoundation;

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

        public async Task Enable(IXRTrackingMode trackingMode)
        {
            var arPlaneManager = await _arComponentsFactory.Create<ARPlaneManager>();
            trackingMode.ConfigureComponents(arPlaneManager);
        }
    }
}