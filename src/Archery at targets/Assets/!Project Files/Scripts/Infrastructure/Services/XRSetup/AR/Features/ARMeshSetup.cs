using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup.AR.Features
{
    public class ARMeshSetup : BaseARSetup<ARMeshManager>
    {
        [Inject]
        public ARMeshSetup(ARMeshManager arMeshManager) : base(arMeshManager)
        {
        }

        public override ARFeature Feature => ARFeature.Mesh;
    }
}