using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure.Services.ARPlanes
{
    public class ARPlanesService : IARPlanesService
    {
        public event Action OnPlaneDetected;

        public bool IsPlaneDetected => _planes.Count > 0;

        private readonly List<ARPlane> _planes = new();

        private ARPlaneManager _planeManager;

        public void SetArPlaneManager(ARPlaneManager planeManager)
        {
            if (_planeManager != null)
            {
                _planeManager.planesChanged -= OnTrackablesChanged;
            }

            _planeManager = planeManager;

            if (_planeManager != null)
            {
                _planeManager.planesChanged += OnTrackablesChanged;
            }

            _planes.Clear();

            CheckForPlanes();
        }

        public ReadOnlyCollection<ARPlane> GetPlanes(PlaneClassification classification)
        {
            var planes = _planes.Where(plane => plane.classification == classification);

            return planes.ToList().AsReadOnly();
        }

        private void OnTrackablesChanged(ARPlanesChangedEventArgs args)
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
                    _planes.Remove(plane);
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