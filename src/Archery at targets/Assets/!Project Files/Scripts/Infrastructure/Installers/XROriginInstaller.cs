using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Installers
{
    public class XROriginInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindARComponents();
        }

        private void BindARComponents()
        {
            Container.Bind<ARSession>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ARCameraManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ARCameraBackground>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ARPlaneManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ARBoundingBoxManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ARAnchorManager>().FromComponentInHierarchy().AsSingle();
            // Container.Bind<ARMeshManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}