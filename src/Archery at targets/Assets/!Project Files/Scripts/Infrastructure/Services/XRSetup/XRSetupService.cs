using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factories.ARComponents;
using Infrastructure.Services.XRSetup.TrackingMode;
using Infrastructure.Services.XRSetup.TrackingModeHandler;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace Infrastructure.Services.XRSetup
{
    [UsedImplicitly]
    public class XRSetupService : IXRSetupService
    {
        private readonly IARComponentsFactory _arComponentsFactory;
        private readonly Dictionary<Type, IXRTrackingModeHandler> _trackingModeHandlers;

        private XRMode _currentMode = XRMode.None;
        private IXRTrackingMode _currentTrackingMode = new NoneTrackingMode();

        public XRSetupService(IARComponentsFactory arComponentsFactory)
        {
            _arComponentsFactory = arComponentsFactory;
            _trackingModeHandlers = new Dictionary<Type, IXRTrackingModeHandler>
            {
                { typeof(NoneTrackingMode), new NoneTrackingModeHandler() },
                { typeof(PlaneTrackingMode), new PlaneTrackingModeHandler(_arComponentsFactory) },
                { typeof(BoundingBoxTrackingMode), new BoundingBoxTrackingModeHandler(_arComponentsFactory) },
                {
                    typeof(PlaneAndBoundingBoxTrackingMode),
                    new PlaneAndBoundingBoxTrackingModeHandler(_arComponentsFactory)
                },
                { typeof(MeshTrackingMode), new MeshTrackingModeHandler(_arComponentsFactory) },
            };
        }

        public void SetXRMode(XRMode mode)
        {
            if (_currentMode == mode) return;

            _currentMode = mode;

            SetComponentState<ARSession>(mode == XRMode.MR);
            SetComponentState<ARInputManager>(mode == XRMode.MR);
            SetComponentState<ARCameraManager>(mode == XRMode.MR);
        }

        public void SetXRTrackingMode(IXRTrackingMode xrTrackingMode)
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

        public void SetAnchorManagerState(bool isAnchorManagerEnabled, GameObject anchorPrefab = null)
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

        public void Initialize()
        {
            _arComponentsFactory.Create<ARSession>().enabled = false;
            _arComponentsFactory.Create<ARInputManager>().enabled = false;
            _arComponentsFactory.Create<ARCameraManager>().enabled = false;
        }
    }
}