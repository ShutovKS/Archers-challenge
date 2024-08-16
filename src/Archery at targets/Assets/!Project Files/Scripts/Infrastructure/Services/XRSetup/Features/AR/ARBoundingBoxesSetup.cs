using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.Features.AR
{
    public class ARBoundingBoxesSetup : BaseARSetup<ARBoundingBoxManager>
    {
        [Inject]
        public ARBoundingBoxesSetup(ARBoundingBoxManager arBoundingBoxManager) : base(arBoundingBoxManager)
        {
        }

        public override ARFeature Feature => ARFeature.BoundingBoxes;
    }
}