using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public class PlaneAndBoundingBoxTrackingMode : IXRTrackingMode
    {
        public PlaneAndBoundingBoxTrackingMode(GameObject planePrefab = null,
            PlaneDetectionMode planeDetectionMode = PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical,
            GameObject boundingBoxPrefab = null)
        {
            _planePrefab = planePrefab;
            _planeDetectionMode = planeDetectionMode;
            _boundingBoxPrefab = boundingBoxPrefab;
        }

        private readonly GameObject _planePrefab;
        private readonly PlaneDetectionMode _planeDetectionMode;
        private readonly GameObject _boundingBoxPrefab;

        public void ConfigureComponents(params Behaviour[] components)
        {
            foreach (var component in components)
            {
                if (component is ARPlaneManager arPlaneManager)
                {
                    arPlaneManager.planePrefab = _planePrefab;
                    arPlaneManager.requestedDetectionMode = _planeDetectionMode;
                }
                else if (component is ARBoundingBoxManager arBoundingBoxManager)
                {
                    arBoundingBoxManager.boundingBoxPrefab = _boundingBoxPrefab;
                }
            }
        }
    }
}