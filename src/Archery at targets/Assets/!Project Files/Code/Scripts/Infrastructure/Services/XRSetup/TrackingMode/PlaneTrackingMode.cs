using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public class PlaneTrackingMode : IXRTrackingMode
    {
        public PlaneTrackingMode(GameObject planePrefab = null,
            PlaneDetectionMode planeDetectionMode = PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical)
        {
            _planePrefab = planePrefab;
            _planeDetectionMode = planeDetectionMode;
        }

        private readonly GameObject _planePrefab;
        private readonly PlaneDetectionMode _planeDetectionMode;

        public void ConfigureComponents(params Behaviour[] components)
        {
            foreach (var component in components)
            {
                if (component is ARPlaneManager arPlaneManager)
                {
                    arPlaneManager.planePrefab = _planePrefab;
                    arPlaneManager.requestedDetectionMode = _planeDetectionMode;
                }
            }
        }
    }
}