using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Installers
{
    public class XROriginInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab;

        public override void InstallBindings()
        {
            var player = InstancePlayer();

            BindARComponents(player);
        }

        private GameObject InstancePlayer()
        {
            var player = Instantiate(playerPrefab);
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;
            DontDestroyOnLoad(player);
            return player;
        }
        
        private void BindARComponents(GameObject player)
        {
            var arSession = player.GetComponentInChildren<ARSession>();
            Container.Bind<ARSession>().FromInstance(arSession).AsSingle();

            var arCamera = player.GetComponentInChildren<Camera>();
            Container.Bind<Camera>().FromInstance(arCamera).AsSingle();
            
            var arCameraManager = player.GetComponentInChildren<ARCameraManager>();
            Container.Bind<ARCameraManager>().FromInstance(arCameraManager).AsSingle();
            
            var arCameraBackground = player.GetComponentInChildren<ARCameraBackground>();
            Container.Bind<ARCameraBackground>().FromInstance(arCameraBackground).AsSingle();
            
            var arPlaneManager = player.GetComponentInChildren<ARPlaneManager>();
            Container.Bind<ARPlaneManager>().FromInstance(arPlaneManager).AsSingle();
            
            var arBoundingBoxManager = player.GetComponentInChildren<ARBoundingBoxManager>();
            Container.Bind<ARBoundingBoxManager>().FromInstance(arBoundingBoxManager).AsSingle();
            
            var arAnchorManager = player.GetComponentInChildren<ARAnchorManager>();
            Container.Bind<ARAnchorManager>().FromInstance(arAnchorManager).AsSingle();
            
            var arMeshManager = player.GetComponentInChildren<ARMeshManager>();
            Container.Bind<ARMeshManager>().FromInstance(arMeshManager).AsSingle();
            
            // var arSessionSetup = player.GetComponentInChildren<ARSessionSetup>();
            // Container.Bind<ARSessionSetup>().FromInstance(arSessionSetup).AsSingle();
            
        }
    }
}