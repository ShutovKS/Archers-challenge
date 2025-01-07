#region

using System;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Factories.ARTrackingMode;
using Infrastructure.Services.Camera;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

#endregion

namespace Infrastructure.Services.XRSetup
{
    public class XRSetupService : IXRSetupService
    {
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly ICameraService _cameraService;
        private readonly IArTrackingModeFactory _arTrackingModeFactory;

        private XRMode _currentMode = XRMode.None;
        private IArTrackingMode _currentIarTrackingMode = new NoneArTrackingMode();

        public XRSetupService(IARComponentsFactory arComponentsFactory, ICameraService cameraService,
            IArTrackingModeFactory arTrackingModeFactory)
        {
            _arComponentsFactory = arComponentsFactory;
            _cameraService = cameraService;
            _arTrackingModeFactory = arTrackingModeFactory;
        }

        public void SetXRMode(XRMode mode)
        {
            if (_currentMode == mode) return;

            _currentMode = mode;

            switch (mode)
            {
                case XRMode.None:
                case XRMode.VR:
                {
                    SetComponentState<ARSession>(false);
                    SetComponentState<ARCameraManager>(false);

                    var trackingMode = _arTrackingModeFactory.Create<NoneArTrackingMode>();
                    SetXRTrackingMode(trackingMode);
                    SetAnchorManagerState(false);

                    _cameraService.SetBackgroundType(CameraBackgroundType.Skybox);
                }
                    break;
                case XRMode.MR:
                {
                    SetComponentState<ARSession>(true);
                    // var arSession = GetOrCreateComponent<ARSession>();
                    // arSession.Reset();

                    SetComponentState<ARCameraManager>(true);

                    var trackingMode = _arTrackingModeFactory.Create<PlaneArTrackingMode>();
                    SetXRTrackingMode(trackingMode);
                    SetAnchorManagerState(true);

                    _cameraService.SetBackgroundType(CameraBackgroundType.SolidColor, Color.clear);
                }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            SetComponentState<ARInputManager>(true);
        }

        private void SetXRTrackingMode(IArTrackingMode iarTrackingMode)
        {
            _currentIarTrackingMode?.Disable();
            _currentIarTrackingMode = iarTrackingMode;
            _currentIarTrackingMode.Enable();
        }

        private void SetComponentState<T>(bool enabled) where T : Behaviour
        {
            var component = GetOrCreateComponent<T>();
            component.enabled = enabled;
        }

        private void SetAnchorManagerState(bool isAnchorManagerEnabled, GameObject anchorPrefab = null)
        {
            if (!isAnchorManagerEnabled)
            {
                _arComponentsFactory.Remove<ARAnchorManager>();
                return;
            }

            var arAnchorManager = _arComponentsFactory.Create<ARAnchorManager>();
            if (anchorPrefab)
            {
                arAnchorManager.anchorPrefab = anchorPrefab;
            }
        }

        private T GetOrCreateComponent<T>() where T : Behaviour =>
            _arComponentsFactory.Get<T>() ?? _arComponentsFactory.Create<T>();
    }
}