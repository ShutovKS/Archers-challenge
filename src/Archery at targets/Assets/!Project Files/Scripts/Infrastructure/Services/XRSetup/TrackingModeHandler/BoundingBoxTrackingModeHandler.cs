using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class BoundingBoxTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;

        public BoundingBoxTrackingModeHandler(IARComponentsFactory arComponentsFactory)
        {
            _arComponentsFactory = arComponentsFactory;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARBoundingBoxManager>();
        }

        public void Enable(IXRTrackingMode trackingMode)
        {
            var arBoundingBoxManager = _arComponentsFactory.Create<ARBoundingBoxManager>();
            trackingMode.ConfigureComponents(arBoundingBoxManager);
        }
    }
}