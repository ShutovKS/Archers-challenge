using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Providers.AssetsAddressables;
using Infrastructure.Services.ARPlanes;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static Data.Constants.Paths.AddressablesPaths;


namespace Infrastructure.Factories.ARTrackingMode
{
    public interface IArTrackingMode
    {
        Task Enable();
        void Disable();
    }

    public abstract class BaseArTrackingMode<TManager, TAsset> : IArTrackingMode where TManager : Behaviour
    {
        protected readonly IARComponentsFactory _arComponentsFactory;
        protected readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        protected BaseArTrackingMode(IARComponentsFactory arComponentsFactory,
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

    public class MeshArTrackingMode : BaseArTrackingMode<ARMeshManager, MeshFilter>
    {
        public MeshArTrackingMode(IARComponentsFactory arComponentsFactory,
            IAssetsAddressablesProvider assetsAddressablesProvider)
            : base(arComponentsFactory, assetsAddressablesProvider)
        {
        }

        protected override async Task ConfigureManager(ARMeshManager manager) => manager.meshPrefab =
            await _assetsAddressablesProvider.GetAsset<MeshFilter>(AR_MESH_PREFAB);
    }

    public class PlaneArTrackingMode : BaseArTrackingMode<ARPlaneManager, GameObject>
    {
        private const PlaneDetectionMode PLANE_DETECTION_MODE =
            PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical;

        private readonly IARPlanesService _arPlanesService;

        public PlaneArTrackingMode(IARComponentsFactory arComponentsFactory, IARPlanesService arPlanesService,
            IAssetsAddressablesProvider assetsAddressablesProvider)
            : base(arComponentsFactory, assetsAddressablesProvider)
        {
            _arPlanesService = arPlanesService;
        }

        protected override async Task ConfigureManager(ARPlaneManager manager)
        {
            _arPlanesService.SetArPlaneManager(manager);
            manager.planePrefab = await _assetsAddressablesProvider.GetAsset<GameObject>(AR_PLANE_PREFAB);
            manager.requestedDetectionMode = PLANE_DETECTION_MODE;
        }

        public override void Disable()
        {
            base.Disable();
            _arPlanesService.SetArPlaneManager(null);
        }
    }

    public class BoundingBoxArTrackingMode : BaseArTrackingMode<ARBoundingBoxManager, GameObject>
    {
        public BoundingBoxArTrackingMode(IARComponentsFactory arComponentsFactory,
            IAssetsAddressablesProvider assetsAddressablesProvider)
            : base(arComponentsFactory, assetsAddressablesProvider)
        {
        }

        protected override async Task ConfigureManager(ARBoundingBoxManager manager) => manager.boundingBoxPrefab =
            await _assetsAddressablesProvider.GetAsset<GameObject>(AR_BOUNDING_BOX_PREFAB);
    }

    public class NoneArTrackingMode : IArTrackingMode
    {
        public Task Enable() => Task.CompletedTask;

        public void Disable()
        {
        }
    }
}