using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public class PlaneAndBoundingBoxTrackingMode : IXRTrackingMode
    {
        public GameObject PlanePrefab { get; set; }

        public PlaneDetectionMode PlaneDetectionMode { get; set; } =
            PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical;

        public GameObject BoundingBoxPrefab { get; set; }

        public void ConfigureComponents(params Behaviour[] components)
        {
            foreach (var component in components)
            {
                if (component is ARPlaneManager arPlaneManager)
                {
                    arPlaneManager.planePrefab = PlanePrefab;
                    arPlaneManager.requestedDetectionMode = PlaneDetectionMode;
                }
                else if (component is ARBoundingBoxManager arBoundingBoxManager)
                {
                    arBoundingBoxManager.boundingBoxPrefab = BoundingBoxPrefab;
                }
            }
        }
    }
}