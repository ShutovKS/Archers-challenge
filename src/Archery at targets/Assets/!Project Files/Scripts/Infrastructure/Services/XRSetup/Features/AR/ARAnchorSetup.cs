using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.Features.AR
{
    public class ARAnchorSetup : BaseARSetup<ARAnchorManager>
    {
        [Inject]
        public ARAnchorSetup(ARAnchorManager arAnchorManager) : base(arAnchorManager)
        {
        }

        public override ARFeature Feature => ARFeature.Anchor;
    }
}