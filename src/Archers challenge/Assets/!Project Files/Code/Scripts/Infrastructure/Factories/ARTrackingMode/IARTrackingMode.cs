using System.Threading.Tasks;
using Data.Constants.Paths;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Providers.AssetsAddressables;
using Infrastructure.Services.ARPlanes;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


namespace Infrastructure.Factories.ARTrackingMode
{
    public interface IARTrackingMode
    {
        Task Enable();
        void Disable();
    }

    public abstract class BaseIarTrackingMode<TManager, TAsset> : IARTrackingMode where TManager : Behaviour
    {
        protected readonly IARComponentsFactory _arComponentsFactory;
        protected readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        protected BaseIarTrackingMode(IARComponentsFactory arComponentsFactory,
            IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _arComponentsFactory = arComponentsFactory;
            _assetsAddressablesProvider = assetsAddressablesProvider;
        }

        public virtual async Task Enable()
        {
            var manager = _arComponentsFactory.Create<TManager>();
            await ConfigureManager(manager);
        }

        public virtual void Disable()
        {
            _arComponentsFactory.Remove<TManager>();
        }

        protected abstract Task ConfigureManager(TManager manager);
    }

    public class MeshIarTrackingMode : BaseIarTrackingMode<ARMeshManager, MeshFilter>
    {
        public MeshIarTrackingMode(IARComponentsFactory arComponentsFactory,
            IAssetsAddressablesProvider assetsAddressablesProvider)
            : base(arComponentsFactory, assetsAddressablesProvider)
        {
        }

        protected override async Task ConfigureManager(ARMeshManager manager) => manager.meshPrefab =
            await _assetsAddressablesProvider.GetAsset<MeshFilter>(AddressablesPaths.AR_MESH_PREFAB);
    }

    public class PlaneIarTrackingMode : BaseIarTrackingMode<ARPlaneManager, GameObject>
    {
        private readonly PlaneDetectionMode _planeDetectionMode =
            PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical;

        private readonly IARPlanesService _arPlanesService;

        public PlaneIarTrackingMode(IARComponentsFactory arComponentsFactory, IARPlanesService arPlanesService,
            IAssetsAddressablesProvider assetsAddressablesProvider)
            : base(arComponentsFactory, assetsAddressablesProvider)
        {
            _arPlanesService = arPlanesService;
        }

        protected override async Task ConfigureManager(ARPlaneManager manager)
        {
            _arPlanesService.SetArPlaneManager(manager);
            manager.planePrefab =
                await _assetsAddressablesProvider.GetAsset<GameObject>(AddressablesPaths.AR_PLANE_PREFAB);
            manager.requestedDetectionMode = _planeDetectionMode;
        }

        public override void Disable()
        {
            base.Disable();
            _arPlanesService.SetArPlaneManager(null);
        }
    }

    public class NoneIarTrackingMode : IARTrackingMode
    {
        public Task Enable() => Task.CompletedTask;

        public void Disable()
        {
        }
    }
}