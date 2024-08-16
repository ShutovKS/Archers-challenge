using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.Features.AR
{
    public class ARSessionSetup : BaseARSetup<ARSession>
    {
        [Inject]
        public ARSessionSetup(ARSession arSession) : base(arSession)
        {
        }

        public override ARFeature Feature => ARFeature.Session;
    }
}