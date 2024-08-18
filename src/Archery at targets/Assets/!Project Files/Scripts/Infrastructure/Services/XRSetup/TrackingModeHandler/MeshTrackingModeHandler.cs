using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.XRSetup.TrackingMode;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup.TrackingModeHandler
{
    public class MeshTrackingModeHandler : IXRTrackingModeHandler
    {
        private readonly IARComponentsFactory _arComponentsFactory;

        public MeshTrackingModeHandler(IARComponentsFactory arComponentsFactory)
        {
            _arComponentsFactory = arComponentsFactory;
        }

        public void Disable()
        {
            _arComponentsFactory.Remove<ARMeshManager>();
        }

        public void Enable(IXRTrackingMode trackingMode)
        {
            var arMeshManager = _arComponentsFactory.Create<ARMeshManager>();
            trackingMode.ConfigureComponents(arMeshManager);
        }
    }
}