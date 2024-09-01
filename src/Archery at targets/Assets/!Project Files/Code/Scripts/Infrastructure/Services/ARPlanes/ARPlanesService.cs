using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.ARPlanes
{
    [UsedImplicitly]
    public class ARPlanesService : IARPlanesService
    {
        public event Action OnPlaneDetected;

        public bool IsPlaneDetected => _planes.Count > 0;

        private readonly List<ARPlane> _planes = new();
        public ReadOnlyCollection<ARPlane> Planes => _planes.AsReadOnly();

        private ARPlaneManager _planeManager;

        public void SetArPlaneManager(ARPlaneManager planeManager)
        {
            if (_planeManager != null)
            {
                _planeManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
            }

            _planeManager = planeManager;

            if (_planeManager != null)
            {
                _planeManager.trackablesChanged.AddListener(OnTrackablesChanged);
            }

            _planes.Clear();

            CheckForPlanes();
        }

        private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
        {
            var planesChanged = false;

            if (args.added != null)
            {
                _planes.AddRange(args.added);
                planesChanged = true;
            }

            if (args.removed != null)
            {
                foreach (var plane in args.removed)
                {
                    _planes.Remove(plane.Value);
                }

                planesChanged = true;
            }

            if (planesChanged)
            {
                OnPlaneDetected?.Invoke();
            }
        }

        private void CheckForPlanes()
        {
            if (_planeManager == null)
            {
                return;
            }

            foreach (var plane in _planeManager.trackables)
            {
                if (!_planes.Contains(plane))
                {
                    _planes.Add(plane);
                }
            }

            if (IsPlaneDetected)
            {
                OnPlaneDetected?.Invoke();
            }
        }
    }
}