#region

using System;
using System.Collections.Generic;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.Camera;
using Infrastructure.Services.XRSetup.TrackingMode;
using Infrastructure.Services.XRSetup.TrackingModeHandler;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

#endregion

namespace Infrastructure.Services.XRSetup
{
    [UsedImplicitly]
    public class XRSetupService : IXRSetupService
    {
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly ICameraService _cameraService;
        private readonly Dictionary<Type, IXRTrackingModeHandler> _trackingModeHandlers;

        private XRMode _currentMode = XRMode.None;
        private IXRTrackingMode _currentTrackingMode = new NoneTrackingMode();

        public XRSetupService(DiContainer container, IARComponentsFactory arComponentsFactory,
            ICameraService cameraService)
        {
            _arComponentsFactory = arComponentsFactory;
            _cameraService = cameraService;
            _trackingModeHandlers = new Dictionary<Type, IXRTrackingModeHandler>
            {
                { typeof(NoneTrackingMode), container.Instantiate<NoneTrackingModeHandler>() },
                { typeof(PlaneTrackingMode), container.Instantiate<PlaneTrackingModeHandler>() },
                { typeof(BoundingBoxTrackingMode), container.Instantiate<BoundingBoxTrackingModeHandler>() },
                { typeof(PlaneAndBoundingBoxTrackingMode), container.Instantiate<PlaneAndBoundingBoxTrackingModeHandler>() },
                { typeof(MeshTrackingMode), container.Instantiate<MeshTrackingModeHandler>() },
            };
        }

        public void SetXRMode(XRMode mode)
        {
            if (_currentMode == mode) return;

            _currentMode = mode;

            switch (mode)
            {
                case XRMode.None:
                case XRMode.VR:
                    SetComponentState<ARSession>(false);
                    SetComponentState<ARCameraManager>(false);
                    SetXRTrackingMode(new NoneTrackingMode());
                    SetAnchorManagerState(false);
                    _cameraService.SetBackgroundType(CameraBackgroundType.Skybox);
                    break;
                case XRMode.MR:
                    SetComponentState<ARSession>(true);
                    var arSession = GetOrCreateComponent<ARSession>();
                    arSession.Reset();

                    SetComponentState<ARCameraManager>(true);

                    SetXRTrackingMode(new PlaneAndBoundingBoxTrackingMode());
                    SetAnchorManagerState(true);

                    _cameraService.SetBackgroundType(CameraBackgroundType.SolidColor, Color.clear);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            SetComponentState<ARInputManager>(true);
        }

        private void SetXRTrackingMode(IXRTrackingMode xrTrackingMode)
        {
            if (_currentTrackingMode.GetType() == xrTrackingMode.GetType()) return;

            DisableCurrentTrackingMode();
            EnableTrackingMode(xrTrackingMode);

            _currentTrackingMode = xrTrackingMode;
        }

        private void SetComponentState<T>(bool enabled) where T : Behaviour
        {
            var component = GetOrCreateComponent<T>();
            component.enabled = enabled;
        }

        private void DisableCurrentTrackingMode()
        {
            if (_trackingModeHandlers.TryGetValue(_currentTrackingMode.GetType(), out var handler))
            {
                handler.Disable();
            }
        }

        private void EnableTrackingMode(IXRTrackingMode xrTrackingMode)
        {
            if (_trackingModeHandlers.TryGetValue(xrTrackingMode.GetType(), out var handler))
            {
                handler.Enable(xrTrackingMode);
            }
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

        private T GetOrCreateComponent<T>() where T : Behaviour
        {
            return _arComponentsFactory.Get<T>() ?? _arComponentsFactory.Create<T>();
        }
    }
}