using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup
{
    [UsedImplicitly]
    public class XRSetupService : IXRSetupService, IInitializable
    {
        private readonly IARComponentsFactory _arComponentsFactory;

        public XRSetupService(IARComponentsFactory arComponentsFactory)
        {
            _arComponentsFactory = arComponentsFactory;
        }

        public async Task SetXRMode(XRMode mode)
        {
            var arSession = await GetXRComponent<ARSession>();
            arSession.enabled = mode == XRMode.MR;

            var arCamera = await GetXRComponent<ARCameraManager>();
            arCamera.enabled = mode == XRMode.MR;
        }

        private async Task<T> GetXRComponent<T>() where T : Behaviour
        {
            var component = _arComponentsFactory.Get<T>();

            if (!component)
            {
                component = await _arComponentsFactory.Create<T>();
            }

            return component;
        }

        public async void Initialize()
        {
            await _arComponentsFactory.Create<ARSession>();
            await _arComponentsFactory.Create<ARCameraManager>();
        }
    }
}