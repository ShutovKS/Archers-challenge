using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.AR.Features
{
    public class ARPlaneSetup : BaseARSetup<ARPlaneManager>
    {
        [Inject]
        public ARPlaneSetup(ARPlaneManager arPlaneManager) : base(arPlaneManager)
        {
        }

        public override ARFeature Feature => ARFeature.Plane;
    }
}