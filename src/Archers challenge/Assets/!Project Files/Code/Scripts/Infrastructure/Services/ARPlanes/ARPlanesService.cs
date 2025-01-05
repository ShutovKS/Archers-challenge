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
                _planeManager.trackablesChanged.AddListener(OnTrackablesChanged);
            }

            _planeManager = planeManager;

            if (_planeManager != null)
            {
                _planeManager.trackablesChanged.AddListener(OnTrackablesChanged);
            }

            _planes.Clear();

            CheckForPlanes();
        }

        public ReadOnlyCollection<ARPlane> GetPlanes(PlaneClassifications classification)
        {
            var planes = _planes.Where(plane => plane.classifications == classification);

            return planes.ToList().AsReadOnly();
        }

        private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> eventArgs)
        {
            var planesChanged = false;

            if (eventArgs.added != null)
            {
                _planes.AddRange(eventArgs.added);
                planesChanged = true;
            }

            if (eventArgs.removed != null)
            {
                foreach (var plane in eventArgs.removed)
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