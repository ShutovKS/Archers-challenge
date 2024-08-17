using Infrastructure.Services.XRSetup;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Installers
{
    public class ARSessionInstaller : MonoInstaller
    {
        [SerializeField] private ARSession aRSession;
        [SerializeField] private ARInputManager arInputManager;

        public override void InstallBindings()
        {
            var xrSetupService = Container.Resolve<IXRSetupService>();

            xrSetupService.AddXRService(aRSession);
            xrSetupService.AddXRService(arInputManager);
        }
    }
}