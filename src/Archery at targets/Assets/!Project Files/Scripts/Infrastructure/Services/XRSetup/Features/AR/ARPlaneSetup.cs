using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.Features.AR
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