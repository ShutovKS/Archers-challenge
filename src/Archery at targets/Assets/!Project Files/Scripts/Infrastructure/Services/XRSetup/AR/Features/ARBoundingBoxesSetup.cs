using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.AR.Features
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