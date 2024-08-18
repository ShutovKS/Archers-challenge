using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Infrastructure.Services.XRSetup.TrackingMode
{
    public class BoundingBoxTrackingMode : IXRTrackingMode
    {
        public BoundingBoxTrackingMode(GameObject boundingBoxPrefab = null)
        {
            _boundingBoxPrefab = boundingBoxPrefab;
        }

        private readonly GameObject _boundingBoxPrefab;

        public void ConfigureComponents(params Behaviour[] components)
        {
            foreach (var component in components)
            {
                if (component is ARBoundingBoxManager arBoundingBoxManager)
                {
                    arBoundingBoxManager.boundingBoxPrefab = _boundingBoxPrefab;
                }
            }
        }
    }
}